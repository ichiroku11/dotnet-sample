using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
				.ToTable(nameof(TodoItem));
		}
	}

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public JsonColumnArrayTest(ITestOutputHelper output) {
		_output = output;

		_context = new(_output);

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
	(2, N'todo-2', N'[""tag-2""]');
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
	public async Task JSON配列の要素を取得できる() {
		// Arrange
		// Act
		var todoItem = await _context.TodoItems
			.Where(item => item.Id == 1)
			.Select(item => new {
				item.Id,
				item.Title,
				// タグの1つ目を取得
				Tag0 = item.Tags[0],
			})
			.FirstAsync();
		// 実行されるクエリ
		// SELECT句において、JSON_VALUE関数が使われている
		/*
		SELECT TOP(1) [t].[Id], [t].[Title], JSON_VALUE([t].[Tags], '$[0]') AS [Tag0]
		FROM [TodoItem] AS [t]
		WHERE [t].[Id] = 1
		*/

		// Assert
		Assert.Equal(1, todoItem.Id);
		Assert.Equal("todo-1", todoItem.Title);
		Assert.Equal("tag-1", todoItem.Tag0);
	}

	[Fact]
	public async Task JSON配列の要素を条件に取得できる() {
		// Arrange
		// Act
		// あまり実用的ではないが、Tagsの1つ目と比較する
		var todoItem = await _context.TodoItems
			.Where(item => item.Tags[0] == "tag-1")
			.FirstAsync();
		// 実行されるクエリ
		// WHERE句においてJSON_VALUE関数が使われている
		/*
		SELECT TOP(1) [t].[Id], [t].[Tags], [t].[Title]
		FROM [TodoItem] AS [t]
		WHERE JSON_VALUE([t].[Tags], '$[0]') = N'tag-1'
		*/

		// Assert
		Assert.Equal(1, todoItem.Id);
		Assert.Equal("todo-1", todoItem.Title);
		Assert.Equal(new[] { "tag-1", "tag-2" }, todoItem.Tags);
	}

	[Fact]
	public async Task JSON配列の要素を含んでいることを条件に取得できる() {
		// Arrange
		// Act
		var todoItems = await _context.TodoItems
			.Where(item => item.Tags.Contains("tag-1"))
			.ToListAsync();
		// 実行されるクエリ
		/*
		SELECT [t].[Id], [t].[Tags], [t].[Title]
		FROM [TodoItem] AS [t]
		WHERE N'tag-1' IN (
			SELECT [t0].[value]
			FROM OPENJSON([t].[Tags]) WITH ([value] nvarchar(max) '$') AS [t0]
		)
		*/

		// Assert
		var todoItem = Assert.Single(todoItems);
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
}
