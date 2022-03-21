using Microsoft.EntityFrameworkCore;

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
		// todo:
	}

	private void DropTable() {
		// todo:
	}
}
