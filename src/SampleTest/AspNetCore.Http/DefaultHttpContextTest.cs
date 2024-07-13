using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace SampleTest.AspNetCore.Http;

public class DefaultHttpContextTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void RequestServices_インスタンスを生成しただけではnull(bool useFeatureCollection) {
		// Arrange
		var context = useFeatureCollection
			? new DefaultHttpContext(new FeatureCollection())
			: new DefaultHttpContext();

		// Act
		// Assert
		Assert.Null(context.RequestServices);
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void RequestBody_コンストラクター引数の有無によってStreamNullになるか例外が発生するか(bool useFeatureCollection) {
		// Arrange
		var context = useFeatureCollection
			? new DefaultHttpContext(new FeatureCollection())
			: new DefaultHttpContext();

		// Act
		// Assert
		Assert.NotNull(context.Request);

		if (useFeatureCollection) {
			// Request.Bodyを取得しようとすると例外
			var exception = Record.Exception(() => context.Request.Body);
			Assert.IsType<NullReferenceException>(exception);
			_output.WriteLine(exception.Message);
		} else {
			// Request.BodyはStream.Null
			Assert.Same(Stream.Null, context.Request.Body);
		}
	}

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void ResponseBody_コンストラクター引数の有無によってStreamNullになるか例外が発生するか(bool useFeatureCollection) {
		// Arrange
		var context = useFeatureCollection
			? new DefaultHttpContext(new FeatureCollection())
			: new DefaultHttpContext();

		// Act
		// Assert
		Assert.NotNull(context.Response);

		if (useFeatureCollection) {
			// Response.Bodyを取得しようとすると例外
			var exception = Record.Exception(() => context.Response.Body);
			Assert.IsType<NullReferenceException>(exception);
			_output.WriteLine(exception.Message);
		} else {
			// Response.BodyはStream.Null
			Assert.Same(Stream.Null, context.Response.Body);
		}
	}
}
