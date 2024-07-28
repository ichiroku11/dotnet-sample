using System.Globalization;

namespace SampleTest;

public class CultureInfoTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	public static TheoryData<string, string, bool, DateTime> GetTheoryData_DateTime_TryParse() {
		var today = DateTime.Today;

		var cultures = new {
			Invariant = "",
			Japanese = "ja-JP",
		};

		return new() {
			// 想定通り
			{ today.ToString("yyyy/MM/dd"), cultures.Japanese, true, today },
			// ちょっと不思議
			{ today.ToString("yyyy/MM/dd"), cultures.Invariant, true, today },

			// 想定通り
			{ today.ToString("yyyy-MM-dd"), cultures.Japanese, true, today },
			// 想定通り
			{ today.ToString("yyyy-MM-dd"), cultures.Invariant, true, today },

			// todo:
			// ちょっと不思議
			{ today.ToString("MM/dd/yyyy"), cultures.Japanese, true, today },
			{ today.ToString("yyyy.MM.dd"), cultures.Japanese, true, today },
			// 想定通り
			{ today.ToString("yyyy年M月d日"), cultures.Japanese, true, today },
			{ today.ToString("yyyy年MM月dd日"), cultures.Japanese, true, today },
		};
	}

	[Theory, MemberData(nameof(GetTheoryData_DateTime_TryParse))]
	public void DateTime_TryParse_文字列を日付に変換できる(string text, string cultureName, bool expectedParsed, DateTime expectedResult) {
		// Arrange
		var culture = string.IsNullOrWhiteSpace(cultureName)
			? CultureInfo.InvariantCulture
			: new CultureInfo("ja-JP");

		// Act
		var actualParsed = DateTime.TryParse(text, culture, out var actualResult);
		_output.WriteLine(actualParsed.ToString());

		// Assert
		Assert.Equal(expectedParsed, actualParsed);
		Assert.Equal(expectedResult, actualResult);
	}
}
