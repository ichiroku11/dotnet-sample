using System.Globalization;

namespace SampleTest;

public class CultureInfoTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	// 想定通り
	[InlineData("2024/07/27", true)]
	// 想定通り
	[InlineData("2024-07-27", true)]
	// ちょっと不思議
	[InlineData("07/27/2024", true)]
	// ちょっと不思議
	[InlineData("2024.07.27", true)]
	// 想定通り
	[InlineData("2024年7月27日", true)]
	public void DateTime_TryParse_JapaneseCulture_文字列を日付に変換できる(string text, bool expected) {
		// Arrange
		var culture = new CultureInfo("ja-JP");

		// Act
		var actual = DateTime.TryParse(text, culture, out var result);
		_output.WriteLine(result.ToString());

		// Assert
		Assert.Equal(expected, actual);
		Assert.Equal(new DateTime(2024, 7, 27), result);
	}
}
