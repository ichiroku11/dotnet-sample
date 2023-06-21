using System.Diagnostics;
using System.Reactive;

namespace SampleTest.Diagnostics;

public class DiagnosticSourceTest {
	[Fact]
	public void IsEnabled_falseになる() {
		// Arrange
		DiagnosticSource source = new DiagnosticListener("Test");

		// Act
		var actual = source.IsEnabled("e");

		// Assert
		Assert.False(actual);
	}
}
