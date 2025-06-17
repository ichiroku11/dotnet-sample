namespace SampleTest;

public class TimeSpanTest {
	[Fact]
	public void Constructor_Days_日付を指定しない場合は0() {
		// Arrange
		// Act
		var actual = new TimeSpan(1, 2, 3).Days;

		// Assert
		Assert.Equal(0, actual);
	}

	public static TheoryData<TimeSpan, TimeSpan, int> GetTheoryData_Compare() {
		return new() {
			// 0の方が大きい
			{ TimeSpan.Zero, new TimeSpan(-1L), 1 },
			// -1の方が大きい
			{ new TimeSpan(-1L), new TimeSpan(-2L), 1 }
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Compare))]
	public void Compare_色々な値を比較する(TimeSpan t1, TimeSpan t2, int expected) {
		// Arrange

		// Act
		var actual = TimeSpan.Compare(t1, t2);

		// Assert
		Assert.Equal(expected, actual);
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
