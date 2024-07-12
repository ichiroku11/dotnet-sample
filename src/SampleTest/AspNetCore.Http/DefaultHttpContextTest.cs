using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SampleTest.AspNetCore.Http;

public class DefaultHttpContextTest {
	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		var context = new DefaultHttpContext();

		// Act
		// Assert
		Assert.Null(context.RequestServices);

		Assert.NotNull(context.Request);
		Assert.Same(Stream.Null, context.Request.Body);

		Assert.NotNull(context.Response);
		Assert.Same(Stream.Null, context.Response.Body);
	}
}
