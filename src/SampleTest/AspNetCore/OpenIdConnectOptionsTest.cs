using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.AspNetCore;

public class OpenIdConnectOptionsTest {
	[Fact]
	public void Properties_値を確認する() {
		// Arrange
		var services = new ServiceCollection();

		services
			.AddAuthentication()
			.AddOpenIdConnect();

		var provider = services.BuildServiceProvider();

		// Act
		var actual = provider.GetRequiredService<IOptions<OpenIdConnectOptions>>().Value;

		// Assert
		Assert.NotNull(actual);
		Assert.Null(actual.Configuration);
		Assert.Null(actual.ConfigurationManager);
	}
}
