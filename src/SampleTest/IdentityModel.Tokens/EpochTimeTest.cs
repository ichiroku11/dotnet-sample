using Microsoft.IdentityModel.Tokens;
using SampleLib;

namespace SampleTest.IdentityModel.Tokens;

public class EpochTimeTest {
	public static TheoryData<long, DateTime> GetTheoryDataForDateTime() {
		return new() {
			{ 0L, EpochTime.UnixEpoch },
			{ 1L, EpochTime.UnixEpoch.AddSeconds(1) },
			// 負の値を指定した場合、Unix EpochTimeになる
			{ -1L, EpochTime.UnixEpoch },
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForDateTime))]
	public void DateTime_秒数をDateTimeに変換する(long second, DateTime expected) {
		// Arrange
		// Act
		var actual = EpochTime.DateTime(second);

		// Assert
		Assert.Equal(expected, actual);

		// UTCになることもついでに確認
		Assert.Equal(DateTimeKind.Utc, actual.Kind);
	}

	public static TheoryData<DateTime, long> GetTheoryDataForGetIntDate() {
		return new() {
			{ EpochTime.UnixEpoch, 0L },
			{ EpochTime.UnixEpoch.AddSeconds(1), 1L },

			// EpochTimeより過去は0になる
			{ EpochTime.UnixEpoch.AddSeconds(-1), 0L },

			// DateTimeKind.LocalのDateTimeをテスト
			// EpochTimeにUTCからの時差を加えて現地時間にして秒数が0になる
			{ new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).Add(TimeZoneInfo.Local.BaseUtcOffset), 0L },
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForGetIntDate))]
	public void GetIntDate_DateTimeをEpochTimeからの経過時間に変換する(DateTime time, long expected) {
		// Arrange
		// Act
		var actual = EpochTime.GetIntDate(time);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]

	public void GetIntDate_DateTimeをEpochTimeからの経過時間に変換するとミリ秒以下が切り捨て足られる() {
		// Arrange
		var now = DateTime.UtcNow;
		var expected = EpochTime.GetIntDate(
			// ミリ秒部分を切り捨てた日時
			now.TruncateMilliseconds());

		// Act
		var actual = EpochTime.GetIntDate(now);

		// Assert
		Assert.Equal(expected, actual);
	}
}
