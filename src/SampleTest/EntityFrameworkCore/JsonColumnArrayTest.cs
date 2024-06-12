using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreTodoItem)]
public class JsonColumnArrayTest : IDisposable {
	private class TodoItem {
		public int Id { get; set; }
		public string Title { get; set; } = "";
		// JSON配列としてシリアライズされる
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
);
insert into dbo.TodoItem(Id, Title, Tags)
output inserted.*
values
	(1, N'todo-1', N'[""tag-1"",""tag-2""]');
";
		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.TodoItem;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task JSON配列を文字列のコレクションとして取得できる() {
		// Arrange
		// Act
		var todoItems = await _context.TodoItems
			.ToListAsync();

		// Assert
		var todoItem = Assert.Single(todoItems);
		Assert.Equal(1, todoItem.Id);
		Assert.Equal("todo-1", todoItem.Title);
		Assert.Equal(new[] { "tag-1", "tag-2" }, todoItem.Tags);
	}
}
