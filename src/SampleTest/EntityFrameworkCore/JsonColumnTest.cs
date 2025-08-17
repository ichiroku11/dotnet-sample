using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreTodoItem)]
public class JsonColumnTest : IDisposable {
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

	public JsonColumnTest(ITestOutputHelper output) {
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
);";
		// todo: insertがうまくいかない

		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.TodoItem;";
		_context.Database.ExecuteSqlRaw(sql);
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
