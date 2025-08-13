using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;

namespace SampleTest.EntityFrameworkCore;

[Collection(CollectionNames.EfCoreBlog)]
public class OwnedEntityStampTest : IDisposable {
	public record Stamp(long By = 0L, DateTime At = new DateTime());

	public class Post {
		public long Id { get; set; }
		public string Title { get; set; } = "";
		public string Content { get; set; } = "";
		public Stamp Created { get; set; } = new Stamp();
		public Stamp Updated { get; set; } = new Stamp();
		public Stamp? Deleted { get; set; }
	}

	public class SampleDbContext(ITestOutputHelper output) : SqlServerDbContext {
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
			modelBuilder.Entity<Post>().ToTable(nameof(Post));

			modelBuilder.Entity<Post>()
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

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public OwnedEntityStampTest(ITestOutputHelper output) {
		_output = output;
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
	Title nvarchar(10) not null,
	Content nvarchar(10) not null,
	CreatedBy bigint not null,
	CreatedAt datetime not null,
	UpdatedBy bigint not null,
	UpdatedAt datetime not null,
	DeletedBy bigint,
	DeletedAt datetime,
	constraint PK_Post primary key(Id));";

		_context.Database.ExecuteSql(sql);

		var now = DateTime.Today;
		_context.Posts.Add(new Post {
			Id = 1L,
			Title = "title1",
			Content = "content1",
			Created = new Stamp { By = 1L, At = now },
			Updated = new Stamp { By = 1L, At = now },
		});
		_context.SaveChanges();
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Post;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task First_正しくエンティティを取得できる() {
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
	}
}
