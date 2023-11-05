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
		// 基準日
		var baseDate = DateTime.Today;
		return new() {
			// 次のxx:05に実行する
			{
				// 例）00:00 => 00:05
				"5 * * * *",
				baseDate,
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 0, 5, 0)
			},
			{
				// 例）01:05 => 02:05
				"5 * * * *",
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 1, 5, 0),
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 2, 5, 0)
			},

			// 次の11:05に実行する
			{
				// 例）11:04 => 当日の11:05
				"5 11 * * *",
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 11, 4, 0),
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 11, 5, 0)
			},
			{
				// 例）11:05 => 翌日の11:05
				"5 11 * * *",
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 11, 5, 0),
				// 翌日
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 11, 5, 0).AddDays(1)
			},

			// 次の1日の11:05
			{
				"5 11 1 * *",
				new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 11, 5, 0),
				// 翌月
				new DateTime(baseDate.Year, baseDate.Month, 1, 11, 5, 0).AddMonths(1)
			},
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
