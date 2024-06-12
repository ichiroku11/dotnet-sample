using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreTodoItem)]
public class JsonColumnArrayTest : IDisposable {
	private class TodoItem {
		public int Id { get; set; }
		public string Title { get; set; } = "";
		public string[] Tags { get; set; } = [];
	}

	// https://learn.microsoft.com/ja-jp/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns
	private class SampleDbContext : SqlServerDbContext {
		public DbSet<TodoItem> TodoItems => Set<TodoItem>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<TodoItem>()
				.ToTable(nameof(TodoItem));
		}
	}

	private readonly SampleDbContext _context = new();

	public JsonColumnArrayTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void InitTable() {
		var sql = @"
create table dbo.TodoItem(
	Id int not null,
	Title nvarchar(10) not null,
	Tags nvarchar(max) not null,
	constraint PK_Blog primary key(Id)
);";
		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.TodoItem;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task とりあえずここまで動くか() {
		// Arrange
		// Act
		var todoItems = await _context.TodoItems
			.ToListAsync();

		// Assert
		Assert.Empty(todoItems);
	}
}
