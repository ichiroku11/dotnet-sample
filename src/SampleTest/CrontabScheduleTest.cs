using NCrontab;

namespace SampleTest;

// NCrontab: Crontab for .NET
// https://github.com/atifaziz/NCrontab
public class CrontabScheduleTest {

	private readonly ITestOutputHelper _output;

	public CrontabScheduleTest(ITestOutputHelper output) {
		_output = output;
	}

	public static TheoryData<string, DateTime, DateTime> GetTheoryData_GetNextOccurrence() {
		var today = DateTime.Today;
		return new() {
			{
				// 5分ごとに実行する
				"*/5 * * * *",
				today,
				today.AddMinutes(5)
			}
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_GetNextOccurrence))]
	public void GetNextOccurrence_色々なパターンを試す(string expression, DateTime start, DateTime expected) {
		// Arrange
		var schedule = CrontabSchedule.Parse(expression);

		// Act
		var actual = schedule.GetNextOccurrence(start);

		// Assert
		Assert.Equal(expected, actual);
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
