using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreBlog)]
public class OwnedEntityStampTest : IDisposable {
	private record Stamp(long By = 0L, DateTime At = new DateTime()) {
		public static readonly Stamp Default = new();
	}

	private class Post {
		public long Id { get; set; }
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
		public Stamp Created { get; set; } = Stamp.Default;
		public Stamp Updated { get; set; } = Stamp.Default;
		public Stamp? Deleted { get; set; }
	}

	private class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
		private readonly ITestOutputHelper _output = output;

		public DbSet<Post> Posts { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.LogTo(
				message => _output.WriteLine(message),
				[DbLoggerCategory.Database.Command.Name],
				LogLevel.Information);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Post>()
				.ToTable(nameof(Post))
				.OwnsOne(entity => entity.Created, navigationBuilder => {
					setColumnName(navigationBuilder, nameof(Post.Created));
				})
				.OwnsOne(entity => entity.Updated, navigationBuilder => {
					setColumnName(navigationBuilder, nameof(Post.Updated));
				})
				.OwnsOne(entity => entity.Deleted, navigationBuilder => {
					setColumnName(navigationBuilder, nameof(Post.Deleted));
				});

			static void setColumnName(OwnedNavigationBuilder<Post, Stamp> builder, string prefix) {
				builder.Property(stamp => stamp.By).HasColumnName($"{prefix}{nameof(Stamp.By)}");
				builder.Property(stamp => stamp.At).HasColumnName($"{prefix}{nameof(Stamp.At)}");
			}
		}
	}

	private readonly SampleDbContext _context;

	public OwnedEntityStampTest(ITestOutputHelper output) {
		_context = new(output);

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
create table dbo.Post(
	Id bigint not null,
	-- TitleとContentの長さは適当
	Title nvarchar(50) not null,
	Content nvarchar(50) not null,
	CreatedBy bigint not null,
	CreatedAt datetime not null,
	UpdatedBy bigint not null,
	UpdatedAt datetime not null,
	DeletedBy bigint,
	DeletedAt datetime,
	constraint PK_Post primary key(Id)
);";

		_context.Database.ExecuteSql(sql);

		var now = DateTime.Today;
		_context.Posts.Add(new Post {
			Id = 1L,
			Title = "title1",
			Content = "content1",
			Created = new(1L, now),
			Updated = new(1L, now),
		});
		_context.SaveChanges();
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Post;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task FirstAsync_正しくエンティティを取得できる() {
		// Arrange
		// Act
		var post = await _context.Posts.FirstAsync();

		// Assert
		Assert.Equal(1L, post.Id);
		Assert.Equal("title1", post.Title);
		Assert.Equal("content1", post.Content);
		Assert.Equal(1L, post.Created.By);
		Assert.Equal(1L, post.Updated.By);
		Assert.True(post.Created.At == post.Updated.At);
		Assert.Null(post.Deleted);
	}

	[Fact]
	public async Task FirstAsync_作成者IDでエンティティを取得できる() {
		// Arrange
		// Act
		var post = await _context.Posts.FirstAsync(post => post.Created.By == 1L);

		// Assert
		Assert.Equal(1L, post.Id);
		Assert.Equal("title1", post.Title);
		Assert.Equal("content1", post.Content);
		Assert.Equal(1L, post.Created.By);
		Assert.Equal(1L, post.Updated.By);
		Assert.True(post.Created.At == post.Updated.At);
		Assert.Null(post.Deleted);
	}

	[Fact]
	public async Task Add_エンティティを追加する() {
		// Arrange
		var now = DateTime.Today.AddHours(1);

		// Act
		_context.Posts.Add(new Post {
			Id = 2L,
			Title = "title2",
			Content = "content2",
			Created = new(2L, now),
			Updated = new(2L, now),
		});
		var changes = await _context.SaveChangesAsync();

		var added = await _context.Posts.FirstAsync(post => post.Id == 2L);

		// Assert
		Assert.Equal(1, changes);

		Assert.Equal(2L, added.Id);
		Assert.Equal("title2", added.Title);
		Assert.Equal("content2", added.Content);
		Assert.Equal(2L, added.Created.By);
		Assert.Equal(now, added.Created.At);
		Assert.Equal(2L, added.Updated.By);
		Assert.Equal(now, added.Updated.At);
		Assert.Null(added.Deleted);
	}

	[Fact]
	public async Task ExecuteUpdateAsync_エンティティを更新する() {
		// Arrange
		var now = DateTime.Today.AddHours(1);

		// Act
		var changes = await _context.Posts
			.Where(p => p.Id == 1L)
			.ExecuteUpdateAsync(
				calls => calls
					.SetProperty(post => post.Title, "title1-updated")
					.SetProperty(post => post.Content, "content1-updated")
					.SetProperty(post => post.Updated.By, 2L)
					.SetProperty(post => post.Updated.At, now));

		var updated = await _context.Posts.FirstAsync(post => post.Id == 1L);

		// Assert
		Assert.Equal(1, changes);

		Assert.Equal(1L, updated.Id);
		Assert.Equal("title1-updated", updated.Title);
		Assert.Equal("content1-updated", updated.Content);
		Assert.Equal(1L, updated.Created.By);
		Assert.False(updated.Created.At == updated.Updated.At);
		Assert.Equal(2L, updated.Updated.By);
		Assert.Equal(now, updated.Updated.At);
		Assert.Null(updated.Deleted);
	}

	[Fact]
	public async Task ExecuteUpdateAsync_エンティティを論理削除する() {
		// Arrange
		var now = DateTime.Today.AddHours(1);

		// Act
		var changes = await _context.Posts
			.Where(p => p.Id == 1L)
			.ExecuteUpdateAsync(
				calls => calls
					// 警告対策で抑制するしかないのか
					.SetProperty(post => post.Deleted!.By, 2L)
					.SetProperty(post => post.Deleted!.At, now));

		var updated = await _context.Posts.FirstAsync(post => post.Id == 1L);

		// Assert
		Assert.Equal(1, changes);

		Assert.Equal(1L, updated.Id);
		Assert.Equal("title1", updated.Title);
		Assert.Equal("content1", updated.Content);
		Assert.Equal(1L, updated.Created.By);
		Assert.True(updated.Created.At == updated.Updated.At);
		Assert.Equal(1L, updated.Updated.By);
		Assert.NotNull(updated.Deleted);
		Assert.Equal(2L, updated.Deleted.By);
		Assert.Equal(now, updated.Deleted.At);
	}
}
