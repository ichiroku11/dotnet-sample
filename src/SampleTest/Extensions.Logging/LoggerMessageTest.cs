using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/logging/loggermessage
public class LoggerMessageTest {
	private static readonly Action<ILogger, Exception?> _log1
		= LoggerMessage.Define(
			LogLevel.Information,
			new EventId(1, nameof(_log1)),
			"log message");
	[Fact]
	public void Define_ログメッセージの出力を確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		_log1(logger, null);

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("log message", logger.Messages.First());
	}
}
