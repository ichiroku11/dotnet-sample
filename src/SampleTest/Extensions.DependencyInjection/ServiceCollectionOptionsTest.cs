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
		Assert.Contains(services, service => service.ServiceType == typeof(IConfigureOptions<SampleOptions>));
		// 登録しただけでは呼び出されない
		Assert.False(configured);
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
		Assert.Contains(services, service => service.ServiceType == typeof(IPostConfigureOptions<SampleOptions>));
		// 登録しただけでは呼び出されない
		Assert.False(postConfigured);
	}
}
