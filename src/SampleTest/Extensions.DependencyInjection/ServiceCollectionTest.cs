using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionTest {
	// 当然だろうが念のため
	[Fact]
	public void Constructor_インスタンスを生成した直後の要素は空() {
		// Arrange
		// Act
		var services = new ServiceCollection();

		// Assert
		Assert.Empty(services);
	}

	private interface IService<TValue> {
	}

	private class Service<TValue> : IService<TValue> {
	}

	[Fact]
	public void AddScoped_ジェネリクス型を登録して取得する() {
		// Arrange
		var services = new ServiceCollection();

		// ジェネリクスはオープン型で登録できる
		services.AddScoped(typeof(IService<>), typeof(Service<>));

		// Act
		// Assert
		var provider = services.BuildServiceProvider();

		// クローズ型を取得できる
		var service1 = provider.GetRequiredService<IService<int>>();
		Assert.IsType<Service<int>>(service1);

		var service2 = provider.GetRequiredService<IService<string>>();
		Assert.IsType<Service<string>>(service2);
	}
}
