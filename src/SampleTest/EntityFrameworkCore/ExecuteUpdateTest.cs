using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace SampleTest.EntityFrameworkCore;

// https://learn.microsoft.com/ja-jp/ef/core/saving/execute-insert-update-delete
[Collection(CollectionNames.EfCoreSample)]
public class ExecuteUpdateTest : IDisposable {
	private class Sample {
		public int Id { get; init; }
		public string Name { get; init; } = "";
	}

	private class SampleDbContext : SqlServerDbContext {
		private readonly ITestOutputHelper _output;

		public SampleDbContext(ITestOutputHelper output) {
			_output = output;
		}

		public DbSet<Sample> Samples => Set<Sample>();

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
			base.OnConfiguring(optionsBuilder);

			// この時点で_outputはnullなので注意
			optionsBuilder.LogTo(
				action: message => _output.WriteLine(message),
				categories: new[] {
					DbLoggerCategory.Database.Command.Name
				},
				minimumLevel: LogLevel.Information);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Sample>().ToTable(nameof(Sample));
		}
	}

	private readonly ITestOutputHelper _output;
	private readonly SampleDbContext _context;

	public ExecuteUpdateTest(ITestOutputHelper output) {
		_output = output;

		_context = new SampleDbContext(_output);
	}

	public void Dispose() {
		_context.Dispose();

		GC.SuppressFinalize(this);
	}
}
