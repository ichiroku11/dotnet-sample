using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 複合主キー、複合外部キーを試す
// 参考
// https://docs.microsoft.com/ja-jp/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-composite-key%2Csimple-key#foreign-key
[Collection(CollectionNames.EfCoreSample)]
public class CompositeKeyTest : IDisposable {
	private class Sample {
		public int Id1 { get; init; }
		public int Id2 { get; init; }
		public string Value { get; init; } = "";
		public List<SampleDetail> Details { get; init; } = [];
	}

	private record SampleDetail(int SampleId1, int SampleId2, int DetailNo, string Value);

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample))
				// 複合主キー
				.HasKey(entity => new { entity.Id1, entity.Id2 });

			modelBuilder.Entity<SampleDetail>().ToTable(nameof(SampleDetail))
				// 複合主キー
				.HasKey(entity => new { entity.SampleId1, entity.SampleId2, entity.DetailNo });
		}
	}

	private readonly SampleDbContext _context = new();

	public CompositeKeyTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void InitTable() {
		FormattableString sql = $@"
create table dbo.Sample(
	Id1 int not null,
	Id2 int not null,
	Value nvarchar(10) not null,
	constraint PK_Sample primary key(Id1, Id2)
);
create table dbo.SampleDetail(
	SampleId1 int not null,
	SampleId2 int not null,
	DetailNo int not null,
	Value nvarchar(10) not null,
	constraint PK_SampleDetail primary key(SampleId1, SampleId2, DetailNo),
	constraint FK_SampleDetail_Sample foreign key(SampleId1, SampleId2) references dbo.Sample(Id1, Id2)
);

insert into dbo.Sample(Id1, Id2, Value)
output inserted.*
values
	(1, 2, N'a');
insert into dbo.SampleDetail(SampleId1, SampleId2, DetailNo, Value)
output inserted.*
values
	(1, 2, 1, N'a-1'),
	(1, 2, 2, N'a-2');";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.SampleDetail;
drop table if exists dbo.Sample;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task FirstOrDefault_関連データなしで取得する() {
		// Arrange
		// Act
		var sample = await _context.Samples
			.FirstAsync(sample => sample.Id1 == 1 && sample.Id2 == 2);

		// Assert
		Assert.Equal(1, sample.Id1);
		Assert.Equal(2, sample.Id2);
		Assert.Equal("a", sample.Value);
		Assert.Empty(sample.Details);
	}

	[Fact]
	public async Task FirstOrDefault_関連データとともに取得する() {
		// Arrange
		// Act
		var sample = await _context.Samples
			.Include(sample => sample.Details)
			.FirstAsync(sample => sample.Id1 == 1 && sample.Id2 == 2);

		// Assert
		Assert.Equal(1, sample.Id1);
		Assert.Equal(2, sample.Id2);
		Assert.Equal("a", sample.Value);
		Assert.Equal(2, sample.Details.Count);
		Assert.Contains(new SampleDetail(1, 2, 1, "a-1"), sample.Details);
		Assert.Contains(new SampleDetail(1, 2, 2, "a-2"), sample.Details);
	}

	[Fact]
	public async Task Add_関連データとともに追加する() {
		// Arrange
		var expected = new Sample {
			Id1 = 1,
			Id2 = 3,
			Value = "b",
			Details = [
				new SampleDetail(1, 3, 1, "b-1"),
				new SampleDetail(1, 3, 2, "b-2"),
			],
		};

		// Act
		_context.Samples.Add(expected);
		var rows = await _context.SaveChangesAsync();

		var actual = await _context.Samples
			.Include(sample => sample.Details)
			.FirstAsync(sample => sample.Id1 == 1 && sample.Id2 == 3);

		// Assert
		Assert.Equal(3, rows);

		Assert.Equal(expected.Id1, actual.Id1);
		Assert.Equal(expected.Id2, actual.Id2);
		Assert.Equal(expected.Value, actual.Value);
		Assert.Equal(
			expected.Details.OrderBy(detail => detail.DetailNo),
			actual.Details.OrderBy(detail => detail.DetailNo));
	}
}
