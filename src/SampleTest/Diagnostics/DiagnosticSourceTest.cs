using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class DiagnosticSourceTest {
	private static DiagnosticSource CreateDiagnosticSource(string name = "Test")
		=> new DiagnosticListener(name);

	[Fact]
	public void IsEnabled_インスタンスを生成しただけではfalseになる() {
		// Arrange
		var source = CreateDiagnosticSource();

		// Act
		var actual = source.IsEnabled("e");

		// Assert
		Assert.False(actual);
	}
}
