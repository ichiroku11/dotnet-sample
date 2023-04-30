using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionOptionsTest {
	private class SampleOptions {
	}

	[Fact]
	public void Configure_IConfigureOptionsが登録されていることを確認する() {
		// Arrange
		var configured = false;
		var services = new ServiceCollection();

		// Act
		services
			.AddOptions<SampleOptions>()
			.Configure(options => {
				configured = true;
			});

		// Assert
		// IConfigureOptionsは登録されるが、
		Assert.Contains(services, service => service.ServiceType == typeof(IConfigureOptions<SampleOptions>));
		// 呼び出されない
		Assert.False(configured);
	}
}
