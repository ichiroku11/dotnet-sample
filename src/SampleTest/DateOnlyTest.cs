namespace SampleTest;

public class DateOnlyTest {

	[Fact]
	public void FromDateTime_DateTimeから生成する() {
		// Arrange
		var now = DateTime.Now;

		// Act
		var date = DateOnly.FromDateTime(now);

		// Assert
		Assert.Equal(now.Year, date.Year);
		Assert.Equal(now.Month, date.Month);
		Assert.Equal(now.Day, date.Day);
	}
}
