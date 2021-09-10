using Microsoft.EntityFrameworkCore;
using SampleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.EntityFrameworkCore {
	public class EntityStateTest : IDisposable {
		private class Sample {
			public int Id { get; init; }
			public string Name { get; init; }
		}

		// InMemory
		private class SampleDbContext : AppDbContext {
			public DbSet<Sample> Samples { get; init; }

			protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
				base.OnConfiguring(optionsBuilder);

				optionsBuilder.UseInMemoryDatabase("sample");
			}

			protected override void OnModelCreating(ModelBuilder modelBuilder) {
				modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
			}
		}

		private SampleDbContext _context;
		private readonly ITestOutputHelper _output;

		public EntityStateTest(ITestOutputHelper output) {
			_context = new SampleDbContext();
			_output = output;
		}

		public void Dispose() {
			if (_context != null) {
				_context.Dispose();
				_context = null;
			}
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

		public static readonly IEnumerable<object[]> TestPatterns = EnumHelper.GetValues<TestPattern>().Select(pattern => new object[] { pattern });

		[Theory]
		[MemberData(nameof(TestPatterns))]
		public void EntityStateがAddedになる(TestPattern pattern) {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

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
		[MemberData(nameof(TestPatterns))]
		public void EntityStateがModifiedになる(TestPattern pattern) {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

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
		[MemberData(nameof(TestPatterns))]
		public void EntityStateがDeletedになる(TestPattern pattern) {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

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
		[MemberData(nameof(TestPatterns))]
		public void EntityStateがUnchangedになる(TestPattern pattern) {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

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
			var entity = new Sample { Id = 1, Name = "a" };

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
}
