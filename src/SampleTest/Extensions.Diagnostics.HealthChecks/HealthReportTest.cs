using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SampleTest.Extensions.Diagnostics.HealthChecks;

public class HealthReportTest {
	[Fact]
	public void Status_値を確認する() {
		// Arrange
		var exptected = HealthStatus.Healthy;
		var entries = new Dictionary<string, HealthReportEntry>();

		// Act
		var actual = new HealthReport(entries, TimeSpan.Zero).Status;

		// Assert
		Assert.Equal(exptected, actual);
	}
}
