using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace SampleTest.DependencyInjection {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1
	// https://qiita.com/TsuyoshiUshio@github/items/20412b36fe63f05671c9
	public class ServiceProviderTest {
		private interface ISampleService {
			string GetValue();
		}

		private class SampleService : ISampleService {
			public string GetValue() => nameof(SampleService);
		}

		private interface ISampleScenario {
			string GetValue();
		}

		private class SampleScenario : ISampleScenario {
			private readonly ISampleService _service;

			public SampleScenario(ISampleService service) {
				_service = service;
			}

			public string GetValue() => $"{nameof(SampleScenario)}.{_service.GetValue()}";
		}


		[Fact]
		public void GetRequiredService_基本的な使い方() {
			// サービス（DIで取得するオブジェクト）の情報を格納するコレクション
			var services = new ServiceCollection();

			// コレクションにサービスを登録
			services.AddTransient<ISampleService, SampleService>();

			// サービスを取得するためのプロバイダーを取得
			var provider = services.BuildServiceProvider();

			// サービスを取得する
			var service = provider.GetRequiredService<ISampleService>();

			Assert.Equal(nameof(SampleService), service.GetValue());
		}

		[Fact]
		public void GetRequiredService_登録されていないサービスを取得するとInvalidOperationException() {
			// Arrange
			var services = new ServiceCollection();
			var provider = services.BuildServiceProvider();

			// Act
			// Assert
			Assert.Throws<InvalidOperationException>(() => provider.GetRequiredService<ISampleService>());
		}

		[Fact]
		public void GetService_登録されていないサービスを取得するとnull() {
			// Arrange
			var services = new ServiceCollection();
			var provider = services.BuildServiceProvider();

			// Act
			var service = provider.GetService<ISampleService>();

			// Assert
			Assert.Null(service);
		}

		[Fact]
		public void GetRequiredService_ネストされたDIも解決できる() {
			// Arange
			var services = new ServiceCollection();
			services.AddTransient<ISampleService, SampleService>();
			services.AddTransient<ISampleScenario, SampleScenario>();

			var provider = services.BuildServiceProvider();

			// Act
			var scenario = provider.GetRequiredService<ISampleScenario>();

			Assert.Equal($"{nameof(SampleScenario)}.{nameof(SampleService)}", scenario.GetValue());
		}
	}
}
