namespace SampleTest;

public class TimeOnlyTest {
	[Fact]
	public void MinValue_各プロパティの値を確認する() {
		// Arrange
		// Act
		var time = TimeOnly.MinValue;

		// Assert
		Assert.Equal(0, time.Hour);
		Assert.Equal(0, time.Minute);
		Assert.Equal(0, time.Second);
		Assert.Equal(0, time.Millisecond);
	}
}
