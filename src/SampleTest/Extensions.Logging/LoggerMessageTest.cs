using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/logging/loggermessage
public class LoggerMessageTest {
	private class TestLogger : ILogger {
		private readonly List<string> _messages = new();

		public IDisposable BeginScope<TState>(TState state) {
			throw new NotImplementedException();
		}

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

	private static readonly Action<ILogger, Exception?> _log
		= LoggerMessage.Define(
			LogLevel.Information,
			new EventId(1, nameof(_log)),
			"This is test message");

	[Fact]
	public void Define_ログメッセージを出力できることを確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		_log(logger, null);

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("This is test message", logger.Messages.First());
	}
}
