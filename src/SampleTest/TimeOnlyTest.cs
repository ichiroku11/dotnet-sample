namespace SampleTest;

public class TimeOnlyTest {
	[Fact]
	public void MinValue_各プロパティの値を確認する() {
		// Arrange
		// Act
		var time = TimeOnly.MinValue;

		// Assert
		Assert.Equal(0, time.Hour);
		Assert.Equal(0, time.Minute);
		Assert.Equal(0, time.Second);
		Assert.Equal(0, time.Millisecond);
	}

	[Fact]
	public void MaxValue_各プロパティの値を確認する() {
		// Arrange
		// Act
		var time = TimeOnly.MaxValue;

		// Assert
		Assert.Equal(23, time.Hour);
		Assert.Equal(59, time.Minute);
		Assert.Equal(59, time.Second);
		Assert.Equal(999, time.Millisecond);
	}

	public static TheoryData<TimeOnly, TimeOnly, TimeOnly, bool> GetTheoryDataForIsBetween() {
		// 参考
		// https://learn.microsoft.com/ja-jp/dotnet/api/system.timeonly.isbetween?view=net-6.0

		return new TheoryData<TimeOnly, TimeOnly, TimeOnly, bool> {
			// 10:00は、09:59～10:01の範囲内
			{ new TimeOnly(10, 0), new TimeOnly(9, 59), new TimeOnly(10, 1), true },

			// 10:00は、10:00～10:01の範囲内
			// 対象時刻が開始時刻と等しい場合はtrue
			{ new TimeOnly(10, 0), new TimeOnly(10, 0), new TimeOnly(10, 1), true },

			// 10:00は、09:59～10:00の範囲内ではない
			// 対象時刻が終了時刻と等しい場合はfalse
			{ new TimeOnly(10, 0), new TimeOnly(9, 59), new TimeOnly(10, 0), false },

			// 00:00は、23:59～00:01の範囲内
			{ new TimeOnly(0, 0), new TimeOnly(23, 59), new TimeOnly(0, 1), true },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForIsBetween))]
	public void IsBetween_動きを確認する(TimeOnly target, TimeOnly start, TimeOnly end, bool expected) {
		// Arrange

		// Act
		var actual = target.IsBetween(start, end);

		// Assert
		Assert.Equal(expected, actual);
	}
}
