using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampleTest.Extensions.DependencyInjection;

// AddXxx/TryAddXxx/TryAddEnumerableの違いを確認する
public class ServiceCollectionTryAddEnumerableTest {
	private interface ISampleService {
	}

	private class SampleService1 : ISampleService {
	}

	private class SampleService2 : ISampleService {
	}

	[Fact]
	public void AddScoped_同じ実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleService1>()
			.AddScoped<ISampleService, SampleService1>();

		// Assert
		Assert.Equal(2, services.Count);
	}

	[Fact]
	public void AddScoped_異なる実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleService1>()
			.AddScoped<ISampleService, SampleService2>();

		// Assert
		Assert.Equal(2, services.Count);
	}

	[Fact]
	public void TryAddScoped_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleService1>()
			.TryAddScoped<ISampleService, SampleService1>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleService1), descriptor.ImplementationType);
	}

	[Fact]
	public void TryAddScoped_異なる実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleService1>()
			.TryAddScoped<ISampleService, SampleService2>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleService1), descriptor.ImplementationType);
	}
}
