using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SampleTest.Extensions.Diagnostics.HealthChecks;

public class HealthReportTest {
	public static TheoryData<IReadOnlyDictionary<string, HealthReportEntry>, HealthStatus> GetTheoryDataForStatus() {
		static HealthReportEntry createEntry(HealthStatus status)
			=> new(status: status, description: default, duration: default, exception: default, data: default);

		return new() {
			// エントリが空の場合はHealthy
			{
				new Dictionary<string, HealthReportEntry>(),
				HealthStatus.Healthy
			},
			// 最も悪いStatusになる
			{
				new Dictionary<string, HealthReportEntry> {
					["a"] = createEntry(HealthStatus.Degraded),
					["b"] = createEntry(HealthStatus.Healthy),
				},
				HealthStatus.Degraded
			},
			{
				new Dictionary<string, HealthReportEntry> {
					["a"] = createEntry(HealthStatus.Degraded),
					["b"] = createEntry(HealthStatus.Unhealthy),
					["c"] = createEntry(HealthStatus.Healthy),
				},
				HealthStatus.Unhealthy
			},
		};
	}

	[Theory, MemberData(nameof(GetTheoryDataForStatus))]
	public void Status_値を確認する(IReadOnlyDictionary<string, HealthReportEntry> entries, HealthStatus expected) {
		// Arrange
		var report = new HealthReport(entries, default);

		// Act
		var actual = report.Status;

		// Assert
		Assert.Equal(expected, actual);
	}
}
