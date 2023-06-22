using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class DiagnosticSourceTest {
	private const string _diagnosticSourceName = "Test";

	private static DiagnosticSource CreateDiagnosticSource(string name = _diagnosticSourceName)
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

	[Fact]
	public void IsEnabled_AllListenersをSubscribeしてもfalseになる() {
		// Arrange
		var source = CreateDiagnosticSource();

		using var subscriber = DiagnosticListener.AllListeners.Subscribe(listener => {
		});

		// Act
		var actual = source.IsEnabled("e");

		// Assert
		Assert.False(actual);
	}
}
