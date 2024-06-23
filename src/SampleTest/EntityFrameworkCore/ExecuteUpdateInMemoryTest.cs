using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

public class ExecuteUpdateInMemoryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class Sample {
		public int Id { get; init; }

		public string Name { get; init; } = "";
	}

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

	[Fact]
	public async Task ExecuteUpdateAsync_インメモリデータベースでは例外が発生する() {
		// Arrange
		using var context = new SampleDbContext();
		context.Samples.Add(new Sample { Id = 1, Name = "abc" });
		await context.SaveChangesAsync();

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await context.Samples
				.Where(sample => sample.Id == 1)
				.ExecuteUpdateAsync(calls => calls.SetProperty(sample => sample.Name, "efg"));
		});

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);
	}
}
