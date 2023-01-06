using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SampleTest.Extensions.Diagnostics.HealthChecks;

public class HealthReportTest {
	public static TheoryData<IReadOnlyDictionary<string, HealthReportEntry>, HealthStatus> GetTheoryDataForStatus() {
		return new() {
			{
				new Dictionary<string, HealthReportEntry>(),
				HealthStatus.Healthy
			},
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForStatus))]
	public void Status_値を確認する(IReadOnlyDictionary<string, HealthReportEntry> entries, HealthStatus expected) {
		// Arrange

		// Act
		var actual = new HealthReport(entries, TimeSpan.Zero).Status;

		// Assert
		Assert.Equal(expected, actual);
	}
}
