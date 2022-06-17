namespace SampleTest;

public class DateTimeOffsetTest {
	[Fact]
	public void UnixEpoch_値を確認する() {
		// Arrange
		// Act
		var unixEpoch = DateTimeOffset.UnixEpoch;

		// Assert
		Assert.Equal(1970, unixEpoch.Year);
		Assert.Equal(1, unixEpoch.Month);
		Assert.Equal(1, unixEpoch.Day);
		Assert.Equal(0, unixEpoch.Hour);
		Assert.Equal(0, unixEpoch.Minute);
		Assert.Equal(0, unixEpoch.Second);
		Assert.Equal(TimeSpan.Zero, unixEpoch.Offset);
	}
}
