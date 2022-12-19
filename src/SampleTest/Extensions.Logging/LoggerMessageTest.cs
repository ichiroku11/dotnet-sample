using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/logging/loggermessage
public class LoggerMessageTest {
	// ログメッセージ
	private static readonly Action<ILogger, Exception?> _log1
		= LoggerMessage.Define(
			LogLevel.Information,
			new EventId(1, nameof(_log1)),
			"log message");

	// プレースホルダーを指定したログメッセージ
	private static readonly Action<ILogger, string, Exception?> _log2
		= LoggerMessage.Define<string>(
			LogLevel.Information,
			new EventId(2, nameof(_log2)),
			"log message: arg='{arg}'");

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

	[Fact]
	public void Define_プレースホルダーを指定したログメッセージの出力を確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		_log2(logger, "arg", null);

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("log message: arg='arg'", logger.Messages.First());
	}
}
