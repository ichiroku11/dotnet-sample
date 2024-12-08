using Microsoft.AspNetCore.Authentication;

namespace SampleTest.AspNetCore.Authentication;

public class AuthenticationPropertiesTest {
	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var properties = new AuthenticationProperties {
		};

		// Assert
		Assert.Null(properties.AllowRefresh);
		Assert.Empty(properties.Items);
		Assert.False(properties.IsPersistent);
		Assert.Empty(properties.Parameters);
	}

	[Fact]
	public void RedirectUri_設定するとItemsに追加される() {
		// Arrange
		// Act
		var properties = new AuthenticationProperties {
			RedirectUri = "redirect-uri"
		};

		// Assert
		var item = Assert.Single(properties.Items);
		Assert.Equal(".redirect", item.Key);
		Assert.Equal("redirect-uri", item.Value);
	}
}
