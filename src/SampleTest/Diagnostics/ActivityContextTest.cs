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
}
