using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

// AddTransient/AddSingleton/AddScopedの違いを確認する
// 参考
// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
public class ServiceCollectionServiceLifetimeTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class Service(Guid value) {
		public Service() : this(Guid.NewGuid()) {
		}

		public Guid Value { get; } = value;
	}

	[Fact]
	public void AddTransient_サービスを要求されるたびに異なるインスタンスが生成される() {
		// Arrange
		var services = new ServiceCollection();
		services.AddTransient<Service>();

		var provider = services.BuildServiceProvider();

		// Act
		var service1 = provider.GetRequiredService<Service>();
		var service2 = provider.GetRequiredService<Service>();

		_output.WriteLine($"{service1.Value}");
		_output.WriteLine($"{service2.Value}");

		// Assert
		Assert.NotSame(service1, service2);
		Assert.NotEqual(service1.Value, service2.Value);
	}

	[Fact(DisplayName = "AddSingleton_サービスを最初に要求されたときに生成され、それ以降は同じインスタンスが取得できる")]
	public void AddSingleton_サービスを最初に要求されたときに生成される() {
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton<Service>();

		var provider = services.BuildServiceProvider();

		// Act
		var service1 = provider.GetRequiredService<Service>();
		var service2 = provider.GetRequiredService<Service>();

		_output.WriteLine($"{service1.Value}");
		_output.WriteLine($"{service2.Value}");

		// Assert
		Assert.Same(service1, service2);
		Assert.Equal(service1.Value, service2.Value);
	}

	[Fact]
	public void AddSingleton_生成したインスタンスを登録する() {
		// Arrange
		var services = new ServiceCollection();
		services.AddSingleton(new Service(Guid.Empty));

		var provider = services.BuildServiceProvider();

		// Act
		var service = provider.GetRequiredService<Service>();

		_output.WriteLine($"{service.Value}");

		// Assert
		Assert.Equal(Guid.Empty, service.Value);
	}

	[Fact]
	public void AddSingleton_サービスを最初に取得するときだけファクトリーメソッドが呼び出される() {
		// Arrange
		var count = 0;
		var services = new ServiceCollection();
		var countSnapshot = new List<int>();

		// Act
		services.AddSingleton(provider => {
			count++;
			return new Service();
		});

		// IServiceProviderを受け取らないFuncを指定すると、
		// Func<Sample>を生成するとして扱われるので注意
		//services.AddSingleton(() => new Sample());
		var provider = services.BuildServiceProvider();

		countSnapshot.Add(count);

		// ファクトリーメソッドが呼び出される
		var actual1 = provider.GetRequiredService<Service>();
		countSnapshot.Add(count);

		// もう1度取得してもファクトリーメソッドは呼び出されない
		var actual2 = provider.GetRequiredService<Service>();
		countSnapshot.Add(count);

		// Assert
		Assert.Equal([0, 1, 1], countSnapshot);
		Assert.Same(actual1, actual2);
		Assert.Equal(actual1.Value, actual2.Value);
	}

	[Fact]
	public void AddScoped_同じスコープ内では同じインスタンスを取得できる() {
		// Arrange
		var services = new ServiceCollection();
		services.AddScoped<Service>();

		var provider = services.BuildServiceProvider();

		// Act
		// Assert
		{
			var service1 = provider.GetRequiredService<Service>();
			_output.WriteLine($"{service1.Value}");

			var service2 = provider.GetRequiredService<Service>();
			_output.WriteLine($"{service2.Value}");

			Assert.Same(service1, service2);
			Assert.Equal(service1.Value, service2.Value);
		}

		using (var scope = provider.CreateScope()) {
			var service1 = scope.ServiceProvider.GetRequiredService<Service>();
			_output.WriteLine($"{service1.Value}");

			var service2 = scope.ServiceProvider.GetRequiredService<Service>();
			_output.WriteLine($"{service2.Value}");

			Assert.Same(service1, service2);
			Assert.Equal(service1.Value, service2.Value);
		}
	}

	[Fact]
	public void AddScoped_異なるスコープでは異なるインスタンスが生成される() {
		// Arrange
		var services = new ServiceCollection();
		services.AddScoped<Service>();

		var provider = services.BuildServiceProvider();

		// Act
		var service1 = default(Service);
		using (var scope = provider.CreateScope()) {
			service1 = scope.ServiceProvider.GetRequiredService<Service>();
		}
		_output.WriteLine($"{service1.Value}");

		var service2 = default(Service);
		using (var scope = provider.CreateScope()) {
			service2 = scope.ServiceProvider.GetRequiredService<Service>();
		}
		_output.WriteLine($"{service2.Value}");

		// Assert
		Assert.NotSame(service1, service2);
		Assert.NotEqual(service1.Value, service2.Value);
	}
}
