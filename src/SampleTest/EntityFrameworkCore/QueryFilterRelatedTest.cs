using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SampleTest.EntityFrameworkCore;

// https://docs.microsoft.com/ja-jp/ef/core/querying/related-data/eager#filtered-include

// グローバルフィルターがIncludeにも適用されるか確認する
// todo:
[Collection(CollectionNames.EfCoreBlog)]
public class QueryFilterRelatedTest : IDisposable {
	private class Blog {
		public int Id { get; init; }
		public string Name { get; init; } = "";
		//public bool Deleted { get; init; }
		public IList<Post> Posts { get; init; } = new List<Post>();
	}

	private class Post {
		public int Id { get; init; }
		public int BlogId { get; init; }
		public string Title { get; init; } = "";
		//public string Content { get; init; } = "";
		public bool Deleted { get; init; }
	}

	private class SampleDbContext : SqlServerDbContext {
		public DbSet<Blog> Blogs => Set<Blog>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Blog>()
				.ToTable(nameof(Blog));

			modelBuilder.Entity<Post>()
				.ToTable(nameof(Post))
				// グローバルフィルター
				.HasQueryFilter(entity => !entity.Deleted);
		}
	}

	private readonly SampleDbContext _context = new();

	public QueryFilterRelatedTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();
	}

	private void InitTable() {
		var sql = @"
create table dbo.Blog(
	Id int not null,
	Name nvarchar(10) not null,
	constraint PK_Blog primary key(Id)
);

create table dbo.Post(
	Id int not null,
	BlogId int not null,
	Title nvarchar(10) not null,
	Deleted bit not null,
	constraint PK_Post primary key(Id),
	constraint FK_Post_Blog foreign key(BlogId) references dbo.Blog(Id)
);

insert into dbo.Blog(Id, Name)
output inserted.*
values
	(1, N'Blog A');

insert into dbo.Post(Id, BlogId, TItle, Deleted)
output inserted.*
values
	(1, 1, N'Post A-1', 0),
	(2, 1, N'Post A-2', 1),
	(3, 1, N'Post A-3', 0);";
		_context.Database.ExecuteSqlRaw(sql);
	}

	private void DropTable() {
		var sql = @"
drop table if exists dbo.Post;
drop table if exists dbo.Blog;";
		_context.Database.ExecuteSqlRaw(sql);
	}

	[Fact]
	public async Task Include_HasQueryFilterを確認する() {
		// Arrange
		// Act
		var blogs = await _context.Blogs
			.Include(blog => blog.Posts.OrderByDescending(post => post.Id))
			.OrderBy(blog => blog.Id)
			.ToListAsync();

		// Assert
		Assert.Single(blogs);
	}
}
