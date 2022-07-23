using Microsoft.IdentityModel.Tokens;

namespace SampleTest.IdentityModel.Tokens;

public class EpochTimeTest {
	public static TheoryData<long, DateTime> GetTheoryDataForDateTime() {
		return new()
		{
			{ 0L, EpochTime.UnixEpoch },
			{ 1L, EpochTime.UnixEpoch.AddSeconds(1) },
			// 負の値を指定した場合、Unix EpochTimeになる
			{ -1L, EpochTime.UnixEpoch },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForDateTime))]
	public void DateTime_秒数をDateTimeに変換する(long second, DateTime expected) {
		// Arrange
		// Act
		var actual = EpochTime.DateTime(second);

		// Assert
		Assert.Equal(expected, actual);

		// UTCになることもついでに確認
		Assert.Equal(DateTimeKind.Utc, actual.Kind);
	}
}
