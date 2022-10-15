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

	[Fact]
	public void MaxValue_各プロパティの値を確認する() {
		// Arrange
		// Act
		var time = TimeOnly.MaxValue;

		// Assert
		Assert.Equal(23, time.Hour);
		Assert.Equal(59, time.Minute);
		Assert.Equal(59, time.Second);
		Assert.Equal(999, time.Millisecond);
	}
}
