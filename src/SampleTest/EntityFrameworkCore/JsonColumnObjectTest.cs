using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SampleTest.EntityFrameworkCore;

// 文字列カラムにJSONオブジェクトのJSON文字列を格納するサンプル
[Collection(CollectionNames.EfCoreTodoItem)]
public class JsonColumnObjectTest : IDisposable {
	// JSONオブジェクトとして扱う
	private class TodoItemDetail {
		public string Note { get; set; } = "";
		public string[] Urls { get; set; } = [];
	}

	private class TodoItem {
		public int Id { get; set; }
		public string Title { get; set; } = "";

		// JSONオブジェクトとしてシリアライズしたい
		public TodoItemDetail Detail { get; set; } = new();
	}

	// https://learn.microsoft.com/ja-jp/ef/core/what-is-new/ef-core-7.0/whatsnew#json-columns
	private class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
		private readonly ITestOutputHelper _output = output;

		public DbSet<TodoItem> TodoItems => Set<TodoItem>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: [DbLoggerCategory.Database.Command.Name],
				minimumLevel: LogLevel.Information);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<TodoItem>()
				.OwnsOne(todoItem => todoItem.Detail, ownedNavigationBuilder => {
					// todo:
					ownedNavigationBuilder.ToJson();
				})
				.ToTable(nameof(TodoItem));
		}
	}

	private readonly SampleDbContext _context;

	public JsonColumnObjectTest(ITestOutputHelper output) {
		_context = new(output);

		DropTable();
		InitTable();
	}

	public void Dispose() {
		//DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void InitTable() {
		var sql = @"
create table dbo.TodoItem(
	Id int not null,
	Title nvarchar(10) not null,
	Detail nvarchar(max) not null,
	constraint PK_TodoItem primary key(Id)
);
insert into dbo.TodoItem(Id, Title, Detail)
output inserted.*
values
	(1, N'todo-1', N'{{""Note"":""note-a"",""Urls"":[""url-a""]}}');
";
		// ↑JSON文字列を格納するには、"{"ではなく"{{"とする必要があるみたい
		// ↓はエラーになる
		// (1, N'todo-1', N'{""Note"":""note-a"",""Urls"":[""url-a""]}');

		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.TodoItem;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task JSONオブジェクトをクラスのインスタンスとして取得できる() {
		// Arrange
		// Act
		var todoItem = await _context.TodoItems
			.OrderBy(item => item.Id)
			.FirstAsync();

		// Assert
		Assert.Equal(1, todoItem.Id);
		Assert.Equal("todo-1", todoItem.Title);
		Assert.Equal("note-a", todoItem.Detail.Note);
		Assert.Equal(["url-a"], todoItem.Detail.Urls);
	}

	[Fact]
	public async Task Add_JSON文字列にシリアライズされてInsertされる() {
		// Arrange
		// Act
		_context.TodoItems.Add(new TodoItem {
			Id = 3,
			Title = "todo-3",
			Detail = new TodoItemDetail {
				Note = "note-c",
				Urls = ["url-c"]
			},
		});
		await _context.SaveChangesAsync();

		// 文字列として取得
		var detail = await _context.Database
			.SqlQuery<string>($"select Detail as Value from dbo.TodoItem where Id = 3")
			.FirstAsync();

		// Assert
		// JSONオブジェクトの文字列としてシリアライズされていることを確認
		Assert.Equal(@"{""Note"":""note-c"",""Urls"":[""url-c""]}", detail);
	}
}
