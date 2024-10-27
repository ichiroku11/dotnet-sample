using Microsoft.AspNetCore.Authentication;

namespace SampleTest.AspNetCore.Authentication;

public class HandleRequestResultTest {
	[Fact]
	public void Fail_プロパティを確認する() {
		// Arrange
		var exception = new Exception();
		var properties = new AuthenticationProperties();

		// Act
		var result = HandleRequestResult.Fail(exception, properties);

		// Assert
		Assert.False(result.Handled);
		Assert.False(result.None);
		Assert.False(result.Skipped);
		Assert.False(result.Succeeded);

		Assert.Same(exception, result.Failure);
		Assert.Same(properties, result.Properties);
	}
}
