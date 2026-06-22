using System.Globalization;

namespace SampleTest;

public class CultureInfoTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	public static TheoryData<string, string> GetTheoryData_DateTime_TryParse() {
		var cultures = new {
			Invariant = "",
			Japanese = "ja-JP",
		};

		return new() {
			// "ja-JP"では想定通りだが、InvariantCultureではちょっと不思議
			{ "yyyy/MM/dd", cultures.Japanese },
			{ "yyyy/MM/dd", cultures.Invariant },

			// どちらも想定通り
			{ "yyyy-MM-dd", cultures.Japanese },
			{ "yyyy-MM-dd", cultures.Invariant },

			// "ja-JP"ではちょっと不思議だが、InvariantCultureでは想定通り
			{ "MM/dd/yyyy", cultures.Japanese },
			{ "MM/dd/yyyy", cultures.Invariant },

			// どちらも不思議といえば不思議
			{ "yyyy.MM.dd", cultures.Japanese },
			{ "yyyy.MM.dd", cultures.Invariant },

			// InvariantCultureでもパースできるのか
			{ "yyyy年M月d日", cultures.Japanese },
			{ "yyyy年M月d日", cultures.Invariant },
			{ "yyyy年MM月dd日", cultures.Japanese },
			{ "yyyy年MM月dd日", cultures.Invariant },
		};
	}

	[Theory, MemberData(nameof(GetTheoryData_DateTime_TryParse))]
	public void DateTime_TryParse_文字列を日付に変換できる(string format, string cultureName) {
		// Arrange
		var expcted = DateTime.Today;

		var text = expcted.ToString(format);

		var culture = string.IsNullOrWhiteSpace(cultureName)
			? CultureInfo.InvariantCulture
			: new CultureInfo(cultureName);
		_output.WriteLine(culture.Name);

		// Act
		var parsed = DateTime.TryParse(text, culture, out var actual);
		_output.WriteLine(parsed.ToString());

		// Assert
		Assert.True(parsed);
		Assert.Equal(expcted, actual);
	}

	public static TheoryData<string, string> GetTheoryData_Convert_ToString() {
		return new() {
			// "ja-JP"
			{ "ja-JP", "yyyy/MM/dd H:mm:ss" },
			// InvariantCulture
			{ "", "MM/dd/yyyy HH:mm:ss" },
		};
	}

	[Theory, MemberData(nameof(GetTheoryData_Convert_ToString))]
	public void Convert_ToString_日付から変換した文字列を確認する(string cultureName, string format) {
		// Arrange
		var today = DateTime.Today;
		var expected = today.ToString(format);

		var culture = string.IsNullOrWhiteSpace(cultureName)
			? CultureInfo.InvariantCulture
			: new CultureInfo(cultureName);
		_output.WriteLine(culture.Name);

		// Act
		var actual = Convert.ToString(today, culture);

		// Assert
		Assert.Equal(expected, actual);
	}
}
