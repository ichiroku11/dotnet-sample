using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.DependencyInjection {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
	public class ServiceCollectionTest {
		private class SampleService {
			public SampleService() : this(Guid.NewGuid()) {
			}

			public SampleService(Guid value) {
				Value = value;
			}

			public Guid Value { get; }
		}


		private readonly ITestOutputHelper _output;

		public ServiceCollectionTest(ITestOutputHelper output) {
			_output = output;
		}


		[Fact]
		public void AddTransient_サービスを要求されるたびに異なるインスタンスが生成される() {
			// Arrange
			var services = new ServiceCollection();
			services.AddTransient<SampleService>();

			var provider = services.BuildServiceProvider();

			// Act
			var service1 = provider.GetRequiredService<SampleService>();
			var service2 = provider.GetRequiredService<SampleService>();

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
			services.AddSingleton<SampleService>();

			var provider = services.BuildServiceProvider();

			// Act
			var service1 = provider.GetRequiredService<SampleService>();
			var service2 = provider.GetRequiredService<SampleService>();

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
			services.AddSingleton(new SampleService(Guid.Empty));

			var provider = services.BuildServiceProvider();

			// Act
			var service = provider.GetRequiredService<SampleService>();

			_output.WriteLine($"{service.Value}");

			// Assert
			Assert.Equal(Guid.Empty, service.Value);
		}

		[Fact]
		public void AddScoped_同じスコープ内では同じインスタンスを取得できる() {
			// Arrange
			var services = new ServiceCollection();
			services.AddScoped<SampleService>();

			var provider = services.BuildServiceProvider();

			// Act
			// Assert
			{
				var service1 = provider.GetRequiredService<SampleService>();
				_output.WriteLine($"{service1.Value}");

				var service2 = provider.GetRequiredService<SampleService>();
				_output.WriteLine($"{service2.Value}");

				Assert.Same(service1, service2);
				Assert.Equal(service1.Value, service2.Value);
			}

			using (var scope = provider.CreateScope()) {
				var service1 = scope.ServiceProvider.GetRequiredService<SampleService>();
				_output.WriteLine($"{service1.Value}");

				var service2 = scope.ServiceProvider.GetRequiredService<SampleService>();
				_output.WriteLine($"{service2.Value}");

				Assert.Same(service1, service2);
				Assert.Equal(service1.Value, service2.Value);
			}
		}

		[Fact]
		public void AddScoped_異なるスコープでは異なるインスタンスが生成される() {
			// Arrange
			var services = new ServiceCollection();
			services.AddScoped<SampleService>();

			var provider = services.BuildServiceProvider();

			// Act
			var service1 = default(SampleService);
			using (var scope = provider.CreateScope()) {
				service1 = scope.ServiceProvider.GetRequiredService<SampleService>();
			}
			_output.WriteLine($"{service1.Value}");

			var service2 = default(SampleService);
			using (var scope = provider.CreateScope()) {
				service2 = scope.ServiceProvider.GetRequiredService<SampleService>();
			}
			_output.WriteLine($"{service2.Value}");

			// Assert
			Assert.NotSame(service1, service2);
			Assert.NotEqual(service1.Value, service2.Value);
		}
	}
}
