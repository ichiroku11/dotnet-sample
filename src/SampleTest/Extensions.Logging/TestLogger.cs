using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

public class TestLogger : ILogger {
	private readonly List<string> _messages = new();

	public IDisposable BeginScope<TState>(TState state)
		=> throw new NotImplementedException();

	public bool IsEnabled(LogLevel logLevel) => true;

	public void Log<TState>(
		LogLevel logLevel,
		EventId eventId,
		TState state,
		Exception? exception,
		Func<TState, Exception?, string> formatter) {
		_messages.Add(formatter(state, exception));
	}

	public IEnumerable<string> Messages => _messages;
}
