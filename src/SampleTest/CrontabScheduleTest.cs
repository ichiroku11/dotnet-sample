using NCrontab;

namespace SampleTest;

public class CrontabScheduleTest {
	[Fact]
	public void NCrontabを使ってみる() {
		// Arrange
		var schedule = CrontabSchedule.Parse("*/5 * * * *");
		var today = DateTime.Today;

		// Act
		var actual = schedule.GetNextOccurrence(today);

		// Assert
		Assert.Equal(today.AddMinutes(5), actual);
	}
}
