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

	// https://learn.microsoft.com/ja-jp/ef/core/what-is-new/ef-core-8.0/whatsnew#enhancements-to-json-column-mapping
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
	(1, N'todo-1', N'[""tag-1"",""tag-2""]'),
	(2, N'todo-2', N'[""tag-2""');
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
		var todoItem = await _context.TodoItems
			.OrderBy(item => item.Id)
			.FirstAsync();

		// Assert
		Assert.Equal(1, todoItem.Id);
		Assert.Equal("todo-1", todoItem.Title);
		Assert.Equal(new[] { "tag-1", "tag-2" }, todoItem.Tags);
	}

	[Fact]
	public async Task JSON配列にシリアライズされてInsertされる() {
		// Arrange
		_context.TodoItems.Add(new TodoItem {
			Id = 3,
			Title = "todo-3",
			Tags = ["tag-1", "tag-3"],
		});
		await _context.SaveChangesAsync();

		// Act
		var tags = await _context.Database
			// 文字列として取得
			.SqlQuery<string>($"select Tags as Value from dbo.TodoItem where Id = 3")
			.FirstAsync();

		// Assert
		// JSON配列の文字列としてシリアライズされていることを確認
		Assert.Equal(@"[""tag-1"",""tag-3""]", tags);
	}


	// todo: タグを含んでいるデータを取得
}
