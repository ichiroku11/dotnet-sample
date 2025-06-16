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

	[Fact]
	public void MaxValue_Days_最大値の値を確認する() {
		// Arrange

		// Act
		var actual = TimeSpan.MaxValue.Days;

		// Assert
		Assert.Equal(10675199, actual);
	}

	[Fact]
	public void MaxValue_TotalDays_最大値の値を確認する() {
		// Arrange

		// Act
		var actual = TimeSpan.MaxValue.TotalDays;

		// Assert
		// intではなくdouble
		Assert.Equal(10675199.116730064, actual);
	}
}
