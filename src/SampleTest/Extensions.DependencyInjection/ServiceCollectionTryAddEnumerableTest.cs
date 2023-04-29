using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampleTest.Extensions.DependencyInjection;

// AddXxx/TryAddXxx/TryAdd/TryAddEnumerableの違いを確認する
public class ServiceCollectionTryAddEnumerableTest {
	private interface ISampleService {
	}

	private class SampleServiceA : ISampleService {
	}

	private class SampleServiceB : ISampleService {
	}

	private readonly ITestOutputHelper _output;

	public ServiceCollectionTryAddEnumerableTest(ITestOutputHelper output) {
		_output = output;
	}

	private void WriteServices(ServiceCollection services) {
		foreach (var service in services) {
			_output.WriteLine(service.ImplementationType?.ToString());
		}
	}

	[Fact]
	public void AddScoped_同じ実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.AddScoped<ISampleService, SampleServiceA>();

		// Assert
		Assert.Equal(2, services.Count);

		WriteServices(services);
	}

	[Fact]
	public void AddScoped_異なる実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.AddScoped<ISampleService, SampleServiceB>();

		// Assert
		Assert.Equal(2, services.Count);

		WriteServices(services);
	}

	[Fact]
	public void TryAddScoped_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAddScoped<ISampleService, SampleServiceA>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddScoped_異なる実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAddScoped<ISampleService, SampleServiceB>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAdd_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAdd(ServiceDescriptor.Scoped<ISampleService, SampleServiceA>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAdd_異なる実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAdd(ServiceDescriptor.Scoped<ISampleService, SampleServiceB>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddEnumerable_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAddEnumerable(ServiceDescriptor.Scoped<ISampleService, SampleServiceA>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(SampleServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddEnumerable_異なる実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<ISampleService, SampleServiceA>()
			.TryAddEnumerable(ServiceDescriptor.Scoped<ISampleService, SampleServiceB>());

		// Assert
		Assert.Equal(2, services.Count);

		WriteServices(services);
	}
}
