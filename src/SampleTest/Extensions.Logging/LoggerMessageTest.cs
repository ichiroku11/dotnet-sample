using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/logging/loggermessage
public class LoggerMessageTest {
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
