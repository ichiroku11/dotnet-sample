using Microsoft.EntityFrameworkCore;
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

		[Fact]
		public void DbContextAdd_StateはAddedになる() {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

			// Act
			_context.Add(entity);

			// Assert
			Assert.Equal(EntityState.Added, _context.Entry(entity).State);
		}

		[Fact]
		public void DbSetAdd_StateはAddedになる() {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

			// Act
			_context.Samples.Add(entity);

			// Assert
			Assert.Equal(EntityState.Added, _context.Entry(entity).State);
		}

		[Fact]
		public void EntryStateSetter_StateはAddedになる() {
			// Arrange
			var entity = new Sample { Id = 1, Name = "a" };

			// Act
			_context.Entry(entity).State = EntityState.Added;

			// Assert
			Assert.Equal(EntityState.Added, _context.Entry(entity).State);
		}
	}
}
