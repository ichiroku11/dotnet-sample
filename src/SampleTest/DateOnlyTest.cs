namespace SampleTest;

public class DateOnlyTest {

	[Fact]
	public void FromDateTime_DateTimeから変換する() {
		// Arrange
		var now = DateTime.Now;

		// Act
		var date = DateOnly.FromDateTime(now);

		// Assert
		Assert.Equal(now.Year, date.Year);
		Assert.Equal(now.Month, date.Month);
		Assert.Equal(now.Day, date.Day);
	}

	[Fact]
	public void ToDateTime_DateTimeに変換する() {
		// Arrange
		var expected = DateTime.Today;

		// Act
		var actual = DateOnly.FromDateTime(expected).ToDateTime(TimeOnly.MinValue);

		// Assert
		Assert.Equal(expected, actual);
	}
}
