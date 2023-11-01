using NCrontab;

namespace SampleTest;

// NCrontab: Crontab for .NET
// https://github.com/atifaziz/NCrontab
public class CrontabScheduleTest {

	private readonly ITestOutputHelper _output;

	public CrontabScheduleTest(ITestOutputHelper output) {
		_output = output;
	}

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

	[Fact]
	public void Parse_オプションを指定しないと秒を含めることができず例外が発生する() {
		// Arrange
		// Act
		var exception = Record.Exception(() => {
			// 秒を含むフォーマットを指定しても例外が発生する
			CrontabSchedule.Parse("* * * * * *", options: null);
		});

		// Assert
		Assert.IsType<CrontabException>(exception);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void Parse_オプションを指定して秒を含めることができる() {
		// Arrange
		var scheduleOptions = new CrontabSchedule.ParseOptions {
			IncludingSeconds = true
		};

		// Act
		var schedule = CrontabSchedule.Parse("* * * * * *", scheduleOptions);

		// Assert
		Assert.NotNull(schedule);
	}
}
