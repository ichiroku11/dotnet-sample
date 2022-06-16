namespace SampleTest;

public class TimeZoneInfoTest {
	[Fact]
	public void BaseUtcOffset_UTCのUTCとの差は0() {
		// Arrange
		// Act
		var actual = TimeZoneInfo.Utc.BaseUtcOffset;

		// Assert
		Assert.Equal(TimeSpan.Zero, actual);
	}
}
