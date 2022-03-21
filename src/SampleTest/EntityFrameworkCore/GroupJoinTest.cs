using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SampleTest.EntityFrameworkCore;

public class GroupJoinTest : IDisposable {
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

	public GroupJoinTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();
	}

	private void InitTable() {
		var sql = @"
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

		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.[Outer];
drop table if exists dbo.[Inner];";

		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task GroupJoin_外部結合を行う() {
		// Arrange
		// Act
		var actuals = await _context.Outers
			.GroupJoin(
				_context.Inners,
				outer => outer.Id,
				inner => inner.Id,
				(outer, inners) => new {
					Outer = outer,
					Inners = inners,
				})
			.SelectMany(
				item => item.Inners.DefaultIfEmpty(),
				(item, inner) => new { item.Outer, Inner = inner })
			.OrderBy(item => item.Outer.Id)
			.ToListAsync();

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
