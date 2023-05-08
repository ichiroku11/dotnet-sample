using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class OptionsBuilderTest {
	private class SampleOptions {
		public int Value { get; set; }
	}

	private readonly ITestOutputHelper _output;

	public OptionsBuilderTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Configure_IConfigureOptionsが登録されていることを確認する() {
		// Arrange
		var services = new ServiceCollection();
		var configured = false;

		// Act
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
				configured = true;
			});

		// Assert
		// IConfigureOptionsが登録されるが
		Assert.Single(services, service => service.ServiceType == typeof(IConfigureOptions<SampleOptions>));
		// 登録しただけでは呼び出されない
		Assert.False(configured);
	}

	[Fact]
	public void Configure_Configureを2回呼び出すとIConfigureOptionsが2つ登録されていることを確認する() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
			})
			.Configure(options => {
			});

		var actual = services.Where(service => service.ServiceType == typeof(IConfigureOptions<SampleOptions>));

		// Assert
		// 2つ登録されている
		Assert.Equal(2, actual.Count());
		// インスタンスも異なる
		Assert.False(actual.First() == actual.Last());
	}

	[Fact]
	public void Configure_OptionsFactory経由でオプションを生成するとConfigureは呼び出される() {
		// Arrange
		var services = new ServiceCollection();
		var configured = false;
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
				_output.WriteLine(nameof(configured));
				configured = true;
			});
		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		Assert.False(configured);
		var options = factory.Create(Options.DefaultName);

		// Assert
		Assert.NotNull(options);
		Assert.True(configured);
	}

	[Fact]
	public void Configure_GetRequiredServiceでオプションを取得してもConfigureは呼び出される() {
		// Arrange
		var services = new ServiceCollection();
		var configured = false;
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
				_output.WriteLine(nameof(configured));
				configured = true;
			});
		var provider = services.BuildServiceProvider();

		// Act
		Assert.False(configured);
		var options = provider.GetRequiredService<IOptions<SampleOptions>>();

		// Assert
		Assert.NotNull(options.Value);
		Assert.True(configured);
	}

	[Fact]
	public void Configure_Configureが2回呼び出されて後がちでオプションが構成されることを確認する() {
		// Arrange
		var services = new ServiceCollection();

		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
				options.Value = -1;
			})
			.Configure(options => {
				options.Value = 1;
			});

		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		var options = factory.Create(Options.DefaultName);

		// Assert
		Assert.Equal(1, options.Value);
	}

	[Fact]
	public void Configure_PostConfigure_呼び出される順番を確認する() {
		// Arrange
		var services = new ServiceCollection();
		var values = new List<string>();

		// Act
		services
			.AddOptions<SampleOptions>()
			.PostConfigure(options => {
				var value = $"pc1-{options.Value}";
				_output.WriteLine(value);
				values.Add(value);

				options.Value++;
			})
			.Configure(options => {
				var value = $"c1-{options.Value}";
				_output.WriteLine(value);
				values.Add(value);

				options.Value++;
			})
			.PostConfigure(options => {
				var value = $"pc2-{options.Value}";
				_output.WriteLine(value);
				values.Add(value);

				options.Value++;
			})
			.Configure(options => {
				var value = $"c2-{options.Value}";
				_output.WriteLine(value);
				values.Add(value);

				options.Value++;
			});

		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		var options = factory.Create(Options.DefaultName);

		// Assert
		// Configure同士、PostConfigure同士では登録した順に実行される
		// PostConfigureはConfigureの後に実行される
		Assert.Collection(values,
			value => Assert.Equal("c1-0", value),
			value => Assert.Equal("c2-1", value),
			value => Assert.Equal("pc1-2", value),
			value => Assert.Equal("pc2-3", value));
		Assert.Equal(4, options.Value);
	}

	[Fact]
	public void PostConfigure_IPostConfigureOptionsが登録されていることを確認する() {
		// Arrange
		var services = new ServiceCollection();
		var postConfigured = false;

		// Act
		services
			.AddOptions<SampleOptions>()
			.PostConfigure(options => {
				postConfigured = true;
			});

		// Assert
		// IPostConfigureOptionsが登録されるが
		Assert.Single(services, service => service.ServiceType == typeof(IPostConfigureOptions<SampleOptions>));
		// 登録しただけでは呼び出されない
		Assert.False(postConfigured);
	}
}
