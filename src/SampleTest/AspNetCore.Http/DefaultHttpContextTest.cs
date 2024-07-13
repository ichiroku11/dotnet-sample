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
		// Request.BodyはStream.Null
		Assert.Same(Stream.Null, context.Request.Body);

		Assert.NotNull(context.Response);
		// Response.BodyはStream.Null
		Assert.Same(Stream.Null, context.Response.Body);
	}

	[Fact]
	public void Properties_FeatureCollectionを指定して生成したインスタンスのプロパティを確認する() {
		// Arrange
		var context = new DefaultHttpContext(new FeatureCollection());

		// Act
		// Assert
		Assert.Null(context.RequestServices);

		Assert.NotNull(context.Request);
		// Request.Bodyを取得しようとすると例外
		var exception = Record.Exception(() => context.Request.Body);
		Assert.IsType<NullReferenceException>(exception);

		Assert.NotNull(context.Response);
		// Response.Bodyを取得しようとすると例外
		exception = Record.Exception(() => context.Response.Body);
		Assert.IsType<NullReferenceException>(exception);
	}
}
