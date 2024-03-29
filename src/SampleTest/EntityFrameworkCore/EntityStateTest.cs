using Microsoft.EntityFrameworkCore;
using SampleLib;

namespace SampleTest.EntityFrameworkCore;

public class EntityStateTest(ITestOutputHelper output) : IDisposable {
	private readonly ITestOutputHelper _output = output;
	private readonly SampleDbContext _context = new();

	private record Sample(int Id, string Name);

	// InMemory
	private class SampleDbContext : AppDbContext {
		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			optionsBuilder.UseInMemoryDatabase("sample");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	// テストパターン
	public enum TestPattern {
		// DbContextのメソッド
		DbContext,
		// DbSetのメソッド
		DbSet,
		// Entryメソッドで取得できるStateプロパティ
		EntryState,
	}

	public static TheoryData<TestPattern> GetTheoryData()
		=> TheoryDataFactory.CreateFrom(EnumHelper.GetValues<TestPattern>());

	[Theory]
	[MemberData(nameof(GetTheoryData))]
	public void EntityStateがAddedになる(TestPattern pattern) {
		// Arrange
		var entity = new Sample(1, "a");

		// Act
		if (pattern == TestPattern.DbContext) {
			_context.Add(entity);
		} else if (pattern == TestPattern.DbSet) {
			_context.Samples.Add(entity);
		} else if (pattern == TestPattern.EntryState) {
			_context.Entry(entity).State = EntityState.Added;
		}

		// Assert
		Assert.Equal(EntityState.Added, _context.Entry(entity).State);
	}

	[Theory]
	[MemberData(nameof(GetTheoryData))]
	public void EntityStateがModifiedになる(TestPattern pattern) {
		// Arrange
		var entity = new Sample(1, "a");

		// Act
		if (pattern == TestPattern.DbContext) {
			_context.Update(entity);
		} else if (pattern == TestPattern.DbSet) {
			_context.Samples.Update(entity);
		} else if (pattern == TestPattern.EntryState) {
			_context.Entry(entity).State = EntityState.Modified;
		}

		// Assert
		Assert.Equal(EntityState.Modified, _context.Entry(entity).State);
	}

	[Theory]
	[MemberData(nameof(GetTheoryData))]
	public void EntityStateがDeletedになる(TestPattern pattern) {
		// Arrange
		var entity = new Sample(1, "a");

		// Act
		if (pattern == TestPattern.DbContext) {
			_context.Remove(entity);
		} else if (pattern == TestPattern.DbSet) {
			_context.Samples.Remove(entity);
		} else if (pattern == TestPattern.EntryState) {
			_context.Entry(entity).State = EntityState.Deleted;
		}

		// Assert
		Assert.Equal(EntityState.Deleted, _context.Entry(entity).State);
	}

	[Theory]
	[MemberData(nameof(GetTheoryData))]
	public void EntityStateがUnchangedになる(TestPattern pattern) {
		// Arrange
		var entity = new Sample(1, "a");

		// Act
		if (pattern == TestPattern.DbContext) {
			_context.Attach(entity);
		} else if (pattern == TestPattern.DbSet) {
			_context.Samples.Attach(entity);
		} else if (pattern == TestPattern.EntryState) {
			_context.Entry(entity).State = EntityState.Unchanged;
		}

		// Assert
		Assert.Equal(EntityState.Unchanged, _context.Entry(entity).State);
	}

	[Fact]
	public void EntityStateがDetachedになる() {
		// Arrange
		var entity = new Sample(1, "a");

		// Act
		// Assert
		// DBコンテキストに関連付いていないのでDetached
		Assert.Equal(EntityState.Detached, _context.Entry(entity).State);

		// DBコンテキストに関連付けるとUnchanged
		_context.Attach(entity);
		Assert.Equal(EntityState.Unchanged, _context.Entry(entity).State);

		// DBコンテキストに関連付けを解除してDetachedになる
		_context.Entry(entity).State = EntityState.Detached;
		Assert.Equal(EntityState.Detached, _context.Entry(entity).State);
	}
}
