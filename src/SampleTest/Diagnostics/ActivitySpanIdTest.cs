using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class ActivitySpanIdTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	[InlineData("new")]
	[InlineData("create-random")]
	public void ToHexString_生成したインスタンスの16進数文字列は16文字(string method) {
		// Arrange
		var spanId = method switch {
			"new" => new ActivitySpanId(),
			"create-random" => ActivitySpanId.CreateRandom(),
			_ => throw new ArgumentOutOfRangeException(nameof(method))
		};

		// Act
		var actual = spanId.ToHexString();
		_output.WriteLine(actual);

		// Assert
		Assert.Equal(16, actual.Length);
	}
}
