using Xunit;

namespace SampleLib.Test;

public class DateTimeExtensionsTest {
	public static TheoryData<DateTime, DateTime> GetTheoryDataForTruncateMilliseconds() {
		// ミリ秒以下を切り捨てる
		static DateTime truncate(DateTime source)
			=> new(source.Year, source.Month, source.Day, source.Hour, source.Minute, source.Second);

		var data = new TheoryData<DateTime, DateTime>();

		var now = DateTime.Now;
		// tickが異なる日時で試す
		// これがベストなテストなのかどうか・・・
		foreach (var ticks in new[] { 0, -1, 1 }) {
			var source = now.AddTicks(ticks);
			data.Add(source, truncate(source));
		}
		return data;
	}

	[Theory, MemberData(nameof(GetTheoryDataForTruncateMilliseconds))]
	public void TruncateMilliseconds_正しく切り捨てできる(DateTime source, DateTime expected) {
		// Arrange
		// Act
		var actual = source.TruncateMilliseconds();

		// Assert
		Assert.Equal(expected, actual);
	}
}
