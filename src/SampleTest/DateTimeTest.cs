namespace SampleTest;

public class DateTimeTest {
	public static TheoryData<DateTime, DateTimeKind> GetTheoryData_Kind() {
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

	[Theory, MemberData(nameof(GetTheoryData_Kind))]
	public void Kind_インスタンスの作り方によって値が変わることを確認する(DateTime target, DateTimeKind expected) {
		// Arrange
		// Act
		// Assert
		Assert.Equal(expected, target.Kind);
	}

	public static TheoryData<DateTime, DateTime> GetTestData_OperatorEquality() {
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

	[Theory, MemberData(nameof(GetTestData_OperatorEquality))]
	public void OperatorEquality_Kindプロパティの違いは無視されて比較される(DateTime left, DateTime right) {
		// Arrange
		// Act
		// Assert
		Assert.True(left == right);
	}

	public static TheoryData<DateTime, DateTimeKind, DateTime> GetTheoryData_SpecifyKind() {
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

	[Theory, MemberData(nameof(GetTheoryData_SpecifyKind))]
	public void SpecifyKind_Kindプロパティの値だけが変更される(DateTime source, DateTimeKind kind, DateTime expected) {
		// Arrange
		// Act
		var actual = DateTime.SpecifyKind(source, kind);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<DateTime, TimeSpan> GetTheoryData_TimeOfDay() {
		return new() {
			{ DateTime.UnixEpoch, new TimeSpan(0, 0, 0) },
			{ new DateTime(2022, 1, 2, 11, 12, 13), new TimeSpan(11, 12, 13) }
		};
	}

	[Theory, MemberData(nameof(GetTheoryData_TimeOfDay))]
	public void TimeOfDay_0時基準の時間間隔を取得できる(DateTime source, TimeSpan expected) {
		// Arrange
		// Act
		var actual = source.TimeOfDay;

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void TryParse_変換失敗した時のresultはMinValueになる() {
		// Arrange
		// Act
		var parsed = DateTime.TryParse("", out var result);

		// Assert
		Assert.False(parsed);
		Assert.Equal(DateTime.MinValue, result);
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
