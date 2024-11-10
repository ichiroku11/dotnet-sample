using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SampleTest.Extensions.DependencyInjection;

// AddXxx/TryAddXxx/TryAdd/TryAddEnumerableの違いを確認する
public class ServiceCollectionTryAddEnumerableTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private interface IService {
	}

	private class ServiceA : IService {
	}

	private class ServiceB : IService {
	}


	private void WriteServices(ServiceCollection services) {
		foreach (var service in services) {
			_output.WriteLine(service.ImplementationType?.Name);
		}
	}

	[Fact]
	public void AddScoped_同じ実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.AddScoped<IService, ServiceA>();

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
			.AddScoped<IService, ServiceA>()
			.AddScoped<IService, ServiceB>();

		// Assert
		Assert.Equal(2, services.Count);

		WriteServices(services);
	}

	[Fact]
	public void AddScoped_同じ実装を複数追加した場合は後勝ちで取得できる様子() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceB>()
			.AddScoped<IService, ServiceA>();

		var provider = services.BuildServiceProvider();

		// Assert
		var service = provider.GetRequiredService<IService>();

		Assert.IsType<ServiceA>(service);
	}

	[Fact]
	public void TryAddScoped_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAddScoped<IService, ServiceA>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(ServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddScoped_異なる実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAddScoped<IService, ServiceB>();

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(ServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAdd_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAdd(ServiceDescriptor.Scoped<IService, ServiceA>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(ServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAdd_異なる実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAdd(ServiceDescriptor.Scoped<IService, ServiceB>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(ServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddEnumerable_同じ実装を複数追加できない() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAddEnumerable(ServiceDescriptor.Scoped<IService, ServiceA>());

		// Assert
		var descriptor = Assert.Single(services);
		Assert.Equal(typeof(ServiceA), descriptor.ImplementationType);

		WriteServices(services);
	}

	[Fact]
	public void TryAddEnumerable_異なる実装を複数追加できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddScoped<IService, ServiceA>()
			.TryAddEnumerable(ServiceDescriptor.Scoped<IService, ServiceB>());

		// Assert
		Assert.Equal(2, services.Count);

		WriteServices(services);
	}
}
