using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class ActivityTraceIdTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData("new")]
	[InlineData("create-random")]
	public void ToHexString_生成したインスタンスの16進数文字列は32文字(string method) {
		// Arrange
		var traceId = method switch {
			"new" => new ActivityTraceId(),
			"create-random" => ActivityTraceId.CreateRandom(),
			_ => throw new ArgumentOutOfRangeException(nameof(method))
		};

		// Act
		var actual = traceId.ToHexString();
		_output.WriteLine(actual);

		// Assert
		Assert.Equal(32, actual.Length);
	}
}
