using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class ActivityContextTest {
	[Fact]
	public void Propeties_インスタンスを生成してプロパティを確認する() {
		// Arrange
		var expected = new {
			TraceId = ActivityTraceId.CreateRandom(),
			SpanId = ActivitySpanId.CreateRandom(),
			TraceFlags = ActivityTraceFlags.Recorded,
			TraceState = "test=1",
		};

		// Act
		var actual = new ActivityContext(expected.TraceId, expected.SpanId, expected.TraceFlags, expected.TraceState);

		// Assert
		Assert.Equal(expected.TraceId, actual.TraceId);
		Assert.Equal(expected.SpanId, actual.SpanId);
		Assert.Equal(expected.TraceFlags, actual.TraceFlags);
		Assert.Equal(expected.TraceState, actual.TraceState);
	}

	[Fact]
	public void Parse_文字列を解析してActivityContextを生成する() {
		// Arrange
		// Act
		var actual = ActivityContext.Parse("00-0123456789abcdef0123456789abcdef-0123456789abcdef-01", "test=1");

		// Assert
		// TraceId：1つ目のハイフンから2つ目のハイフンまで
		// SpanId：2つ目のハイフンから3つ目のハイフンまで
		// TraceFlags：3つ目のハイフンから最後までが
		Assert.Equal("0123456789abcdef0123456789abcdef", actual.TraceId.ToHexString());
		Assert.Equal("0123456789abcdef", actual.SpanId.ToHexString());
		Assert.Equal(ActivityTraceFlags.Recorded, actual.TraceFlags);
		Assert.Equal("test=1", actual.TraceState);
	}
}
