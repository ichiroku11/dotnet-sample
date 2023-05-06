using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionOptionsTest {
	private class SampleOptions {
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
	public void Configure_OptionsFactory経由でオプションを生成するとConfigureは呼び出される() {
		// Arrange
		var services = new ServiceCollection();
		var configured = false;
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
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
