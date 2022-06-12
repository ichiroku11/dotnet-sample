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
			{ DateTime.UnixEpoch, DateTimeKind.Utc }
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForKind))]
	public void Kind_インスタンスの作り方によって値が変わることを確認する(DateTime target, DateTimeKind expected) {
		// Arrange
		// Act
		// Assert
		Assert.Equal(expected, target.Kind);
	}

	[Fact]
	public void UnixEpoch_値を確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(1970, DateTime.UnixEpoch.Year);
		Assert.Equal(1, DateTime.UnixEpoch.Month);
		Assert.Equal(1, DateTime.UnixEpoch.Day);
		Assert.Equal(0, DateTime.UnixEpoch.Hour);
		Assert.Equal(0, DateTime.UnixEpoch.Minute);
		Assert.Equal(0, DateTime.UnixEpoch.Second);
		Assert.Equal(DateTimeKind.Utc, DateTime.UnixEpoch.Kind);
	}
}
