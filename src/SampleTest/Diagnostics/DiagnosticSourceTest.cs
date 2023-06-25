using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class DiagnosticSourceTest {
	private const string _diagnosticSourceName = "Test";

	private static DiagnosticSource CreateDiagnosticSource(string name = _diagnosticSourceName)
		=> new DiagnosticListener(name);

	[Theory]
	[InlineData("")]
	[InlineData("e")]
	public void IsEnabled_インスタンスを生成しただけではfalseになる(string eventName) {
		// Arrange
		var source = CreateDiagnosticSource();

		// Act
		var actual = source.IsEnabled(eventName);

		// Assert
		Assert.False(actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("e")]
	public void IsEnabled_AllListenersをSubscribeしてもfalseになる(string eventName) {
		// Arrange
		var source = CreateDiagnosticSource();

		using var subscriber = DiagnosticListener.AllListeners.Subscribe(listener => {
		});

		// Act
		var actual = source.IsEnabled(eventName);

		// Assert
		Assert.False(actual);
	}

	[Theory]
	[InlineData("")]
	[InlineData("e")]
	public void IsEnabled_AllListenersのlistenerをSubscribeするとtrueになる(string eventName) {
		// Arrange
		var source = CreateDiagnosticSource();

		using var subscriber = DiagnosticListener.AllListeners.Subscribe(listener => {
			if (!string.Equals(listener.Name, _diagnosticSourceName, StringComparison.OrdinalIgnoreCase)) {
				return;
			}

			listener.Subscribe(item => {
			});
		});

		// Act
		var actual = source.IsEnabled(eventName);

		// Assert
		Assert.True(actual);
	}

	[Fact]
	public void Write_書き込みを購読できることを確認する() {
		// Arrange
		var source = CreateDiagnosticSource();
		var actual = new List<KeyValuePair<string, object?>>();

		using var subscriber = DiagnosticListener.AllListeners.Subscribe(listener => {
			if (!string.Equals(listener.Name, _diagnosticSourceName, StringComparison.OrdinalIgnoreCase)) {
				return;
			}

			listener.Subscribe(item => {
				actual.Add(item);
			});
		});

		// Act
		source.Write("x", 1);
		source.Write("y", 2);

		// Assert
		Assert.Collection(actual,
			item => {
				Assert.Equal("x", item.Key);
				Assert.Equal(1, item.Value);
			},
			item => {
				Assert.Equal("y", item.Key);
				Assert.Equal(2, item.Value);
			});
	}
}
