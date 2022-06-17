namespace SampleTest;

public class DateTimeTest {
	public static TheoryData<DateTime, DateTimeKind> GetTheoryDataForKind() {
		return new() {
			// 現地時刻
			{ DateTime.Today, DateTimeKind.Local },
			{ DateTime.Now, DateTimeKind.Local },

			// 未指定
			{ new DateTime(1970, 1, 1), DateTimeKind.Unspecified },
			{ DateTime.Parse("1970/01/01"), DateTimeKind.Unspecified },
			{ DateTime.MinValue, DateTimeKind.Unspecified },
			{ DateTime.MaxValue, DateTimeKind.Unspecified },

			// UTC（世界協定時刻）
			{ DateTime.UnixEpoch, DateTimeKind.Utc },
			{ DateTime.UtcNow, DateTimeKind.Utc }
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForKind))]
	public void Kind_インスタンスの作り方によって値が変わることを確認する(DateTime target, DateTimeKind expected) {
		// Arrange
		// Act
		// Assert
		Assert.Equal(expected, target.Kind);
	}

	public static TheoryData<DateTime, DateTime> GetTestDataForOperatorEquality() {
		// DateTimeKind違いのインスタンスは等しいと判断される
		var local = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		var utc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		var unspecified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);
		return new() {
			{ local, utc },
			{ utc, unspecified },
			{ unspecified, local }
		};
	}
	// todo:
	[Theory, MemberData(nameof(GetTestDataForOperatorEquality))]
	public void OperatorEquality_Kindプロパティの違いは無視されて比較される(DateTime left, DateTime right) {
		// Arrange
		// Act
		// Assert
		Assert.True(left == right);
	}

	public static TheoryData<DateTime, DateTimeKind, DateTime> GetTheoryDataForSpecifyKind() {
		var local = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		var utc = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
		var unspecified = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified);

		return new() {
			{
				local,
				DateTimeKind.Utc,
				utc
			},
			{
				unspecified,
				DateTimeKind.Utc,
				utc
			}
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForSpecifyKind))]
	public void SpecifyKind_Kindプロパティの値だけが変更される(DateTime source, DateTimeKind kind, DateTime expected) {
		// Arrange
		// Act
		var actual = DateTime.SpecifyKind(source, kind);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<DateTime, TimeSpan> GetTheoryDataForTimeOfDay() {
		return new() {
			{ DateTime.UnixEpoch, new TimeSpan(0, 0, 0) },
			{ new DateTime(2022, 1, 2, 11, 12, 13), new TimeSpan(11, 12, 13) }
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForTimeOfDay))]
	public void TimeOfDay_0時基準の時間感覚を取得できる(DateTime source, TimeSpan expected) {
		// Arrange
		// Act
		var actual = source.TimeOfDay;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void UnixEpoch_値を確認する() {
		// Arrange
		// Act
		var unixEpoch = DateTime.UnixEpoch;

		// Assert
		Assert.Equal(1970, unixEpoch.Year);
		Assert.Equal(1, unixEpoch.Month);
		Assert.Equal(1, unixEpoch.Day);
		Assert.Equal(0, unixEpoch.Hour);
		Assert.Equal(0, unixEpoch.Minute);
		Assert.Equal(0, unixEpoch.Second);
		Assert.Equal(DateTimeKind.Utc, unixEpoch.Kind);
	}
}
