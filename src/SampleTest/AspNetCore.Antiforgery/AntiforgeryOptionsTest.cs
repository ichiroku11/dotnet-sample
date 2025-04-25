using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.AspNetCore.Antiforgery;

public class AntiforgeryOptionsTest {
	[Fact]
	public void Properties_デフォルト値を確認する() {
		// Arrange
		var options = new AntiforgeryOptions();

		// Act
		// Assert
		Assert.Equal("__RequestVerificationToken", options.FormFieldName);
		Assert.Equal("RequestVerificationToken", options.HeaderName);
	}

	[Fact]
	public void Properties_AddAntiforgeryで設定した場合のデフォルト値を確認する() {
		// Arrange
		var services = new ServiceCollection();
		services.AddAntiforgery();

		var provider = services.BuildServiceProvider();

		// Act
		var options = provider.GetRequiredService<IOptions<AntiforgeryOptions>>().Value;

		// Assert
		Assert.Equal("__RequestVerificationToken", options.FormFieldName);
		Assert.Equal("RequestVerificationToken", options.HeaderName);
	}
}
