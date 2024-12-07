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
}
