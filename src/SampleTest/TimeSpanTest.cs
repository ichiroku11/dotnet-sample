namespace SampleTest;

public class TimeSpanTest {
	[Fact]
	public void Constructor_日付を指定しない場合はDaysは0() {
		// Arrange
		// Act
		var actual = new TimeSpan(1, 2, 3).Days;

		// Assert
		Assert.Equal(0, actual);
	}
}
