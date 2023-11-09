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
		return new() {
			// 次のxx:05に実行する
			{
				// 例）00:00 => 00:05
				"5 * * * *",
				new DateTime(2023, 9, 1),
				new DateTime(2023, 9, 1, 0, 5, 0)
			},
			{
				// 例）01:05 => 02:05
				"5 * * * *",
				new DateTime(2023, 9, 1, 1, 5, 0),
				new DateTime(2023, 9, 1, 2, 5, 0)
			},

			// 次の11:05に実行する
			{
				// 例）11:04 => 当日の11:05
				"5 11 * * *",
				new DateTime(2023, 9, 1, 11, 4, 0),
				new DateTime(2023, 9, 1, 11, 5, 0)
			},
			{
				// 例）11:05 => 翌日の11:05
				"5 11 * * *",
				new DateTime(2023, 9, 1, 11, 5, 0),
				// 翌日
				new DateTime(2023, 9, 2, 11, 5, 0)
			},

			// 毎月1日の11:05
			{
				"5 11 1 * *",
				new DateTime(2023, 9, 1, 11, 5, 0),
				// 翌月
				new DateTime(2023, 10, 1, 11, 5, 0)
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

	public static TheoryData<string, DateTime, DateTime, IEnumerable<DateTime>> GetTheoryData_GetNextOccurrences() {
		// 基準日
		return new() {
			// xx:05、xx:10、xx:15...と5分間隔で実行する
			{
				"*/5 * * * *",
				new DateTime(2023, 9, 1),
				new DateTime(2023, 9, 1, 0, 20, 0),
				new [] {
					// xx:00（開始時刻）は含まれない
					new DateTime(2023, 9, 1, 0, 5, 0),
					new DateTime(2023, 9, 1, 0, 10, 0),
					new DateTime(2023, 9, 1, 0, 15, 0),
					// xx:20（終了時刻）は含まれない
				}
			},
			{
				// xx:01から開始しても、xx:05、xx:10、xx:15...と実行される
				"*/5 * * * *",
				new DateTime(2023, 9, 1, 0, 1, 0),
				new DateTime(2023, 9, 1, 0, 20, 0),
				new [] {
					new DateTime(2023, 9, 1, 0, 5, 0),
					new DateTime(2023, 9, 1, 0, 10, 0),
					new DateTime(2023, 9, 1, 0, 15, 0),
				}
			},
			// 毎日01:00に実行する
			{
				"0 1 * * *",
				new DateTime(2023, 9, 1),
				new DateTime(2023, 9, 4),
				new [] {
					new DateTime(2023, 9, 1, 1, 0, 0),
					new DateTime(2023, 9, 2, 1, 0, 0),
					new DateTime(2023, 9, 3, 1, 0, 0),
				}
			},
			// 毎週日曜日01:00に実行する
			{
				"0 1 * * 0",
				new DateTime(2023, 9, 1),
				new DateTime(2023, 9, 30),
				new [] {
					new DateTime(2023, 9, 3, 1, 0, 0),
					new DateTime(2023, 9, 10, 1, 0, 0),
					new DateTime(2023, 9, 17, 1, 0, 0),
					new DateTime(2023, 9, 24, 1, 0, 0),
				}
			},
			// 毎週月曜日・金曜日01:00に実行する
			{
				"0 1 * * 1,5",
				new DateTime(2023, 9, 1),
				new DateTime(2023, 9, 30),
				new [] {
					// 月曜
					new DateTime(2023, 9, 4, 1, 0, 0),
					new DateTime(2023, 9, 11, 1, 0, 0),
					new DateTime(2023, 9, 18, 1, 0, 0),
					new DateTime(2023, 9, 25, 1, 0, 0),
				}.Concat(new [] {
					// 金曜
					new DateTime(2023, 9, 1, 1, 0, 0),
					new DateTime(2023, 9, 8, 1, 0, 0),
					new DateTime(2023, 9, 15, 1, 0, 0),
					new DateTime(2023, 9, 22, 1, 0, 0),
					new DateTime(2023, 9, 29, 1, 0, 0),
				}).OrderBy(date => date)
			},

			// todo: 平日01:00に実行する
			// todo: 毎月5日
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_GetNextOccurrences))]
	public void GetNextOccurrences_色々なパターンを試す(string expression, DateTime start, DateTime end, IEnumerable<DateTime> expected) {
		// Arrange
		var schedule = CrontabSchedule.Parse(expression);

		// Act
		var actual = schedule.GetNextOccurrences(start, end);

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
