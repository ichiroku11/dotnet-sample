using System.Globalization;

namespace SampleTest;

public class CultureInfoTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	public static TheoryData<string, DateTime> GetTheoryData_DateTime_TryParse_JapaneseCulture() {
		var today = DateTime.Today;

		return new() {
			// 想定通り
			{ today.ToString("yyyy/MM/dd"), today },
			{ today.ToString("yyyy-MM-dd"), today },
			// ちょっと不思議
			{ today.ToString("MM/dd/yyyy"), today },
			{ today.ToString("yyyy.MM.dd"), today },
			// 想定通り
			{ today.ToString("yyyy年M月d日"), today },
			{ today.ToString("yyyy年MM月dd日"), today },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_DateTime_TryParse_JapaneseCulture))]
	public void DateTime_TryParse_JapaneseCulture_文字列を日付に変換できる(string text, DateTime expected) {
		// Arrange
		var culture = new CultureInfo("ja-JP");

		// Act
		var result = DateTime.TryParse(text, culture, out var actual);
		_output.WriteLine(result.ToString());

		// Assert
		Assert.True(result);
		Assert.Equal(expected, actual);
	}
}
