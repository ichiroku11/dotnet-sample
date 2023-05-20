using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionOptionsTest {
	private class SampleOptions {
	}
	private readonly ITestOutputHelper _output;

	public ServiceCollectionOptionsTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Configure_IConfigureOptionsが登録されていることを確認する() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		services.Configure<SampleOptions>(options => {
		});

		// Assert
		// AddOptionsしなくても
		// IConfigureOptionsが登録されている
		Assert.Single(services, service => service.ServiceType == typeof(IConfigureOptions<SampleOptions>));
	}
}
