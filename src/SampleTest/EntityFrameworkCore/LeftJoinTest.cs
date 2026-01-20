using Microsoft.EntityFrameworkCore;
using Polly;

namespace SampleTest.EntityFrameworkCore;

public class LeftJoinTest : IDisposable {
	public record Outer(int Id, string Value);
	public record Inner(int Id, string Value);

	public class OuterInnerDbContext : SqlServerDbContext {
		public DbSet<Outer> Outers => Set<Outer>();
		public DbSet<Inner> Inners => Set<Inner>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Outer>().ToTable(nameof(Outer));
			modelBuilder.Entity<Inner>().ToTable(nameof(Inner));
		}
	}

	private readonly OuterInnerDbContext _context = new();

	public LeftJoinTest() {
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
create table dbo.[Outer](
	Id int not null,
	Value nvarchar(10) not null,
	constraint PK_Outer primary key(Id)
);
insert into dbo.[Outer](Id, Value)
output inserted.*
values
	(1, N'a'),
	(2, N'b'),
	(3, N'c');

create table dbo.[Inner](
	Id int not null,
	Value nvarchar(10) not null,
	constraint PK_Inner primary key(Id)
);
insert into dbo.[Inner](Id, Value)
output inserted.*
values
	(1, N'A'),
	(4, N'D');";

		_context.Database.ExecuteSql(sql);

		// 投入されたデータに対してleft outer joinすると
		/*
		select *
		from dbo.[Outer]
			left outer join dbo.[Inner]
				on dbo.[Outer].Id = dbo.[Inner].Id
		order by dbo.[Outer].Id;

		Id	Value	Id	Value
		1	a	1	A
		2	b	NULL	NULL
		3	c	NULL	NULL
		*/
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.[Outer];
drop table if exists dbo.[Inner];";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task LeftJoin_外部結合を行う() {
		// Arrange
		// Act
		var actuals = await _context.Outers
			.LeftJoin(
				_context.Inners,
				outer => outer.Id,
				inner => inner.Id,
				(outer, inner) => new { Outer = outer, Inner = inner })
			.OrderBy(item => item.Outer.Id)
			.ToListAsync();
		// 実行されるクエリ
		/*
		SELECT [o].[Id], [o].[Value], [i].[Id], [i].[Value]
		FROM [Outer] AS [o]
		LEFT JOIN [Inner] AS [i] ON [o].[Id] = [i].[Id]
		ORDER BY [o].[Id]
		*/

		// Assert
		Assert.Collection(actuals,
			actual => {
				Assert.Equal(new Outer(1, "a"), actual.Outer);
				Assert.Equal(new Inner(1, "A"), actual.Inner);
			},
			actual => {
				Assert.Equal(new Outer(2, "b"), actual.Outer);
				Assert.Null(actual.Inner);
			},
			actual => {
				Assert.Equal(new Outer(3, "c"), actual.Outer);
				Assert.Null(actual.Inner);
			});
	}
}
