using Microsoft.Extensions.Logging;

namespace SampleTest.Extensions.Logging;

// 属性を使ったコード生成が主流の様子
// https://learn.microsoft.com/ja-jp/dotnet/core/extensions/logger-message-generator
public partial class LoggerMessageAttributeTest {
	[LoggerMessage(
		EventId = 1,
		Level = LogLevel.Information,
		Message = "log message")]
	private static partial void Log1(ILogger logger);

	[LoggerMessage(
		EventId = 2,
		Level = LogLevel.Information,
		Message = "log message: arg='{arg}'")]
	private static partial void Log2(ILogger logger, string arg);

	[LoggerMessage(
		EventId = 3,
		Level = LogLevel.Information,
		Message = "log message: arg='{arg}'")]
	private static partial void Log3(ILogger logger, IEnumerable<string> arg);

	[Fact]
	public void Attribute_ログメッセージの出力を確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		Log1(logger);

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("log message", logger.Messages.First());
	}

	[Fact]
	public void Attribute_プレースホルダーを指定したログメッセージの出力を確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		Log2(logger, "arg");

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("log message: arg='arg'", logger.Messages.First());
	}

	[Fact]
	public void Attribute_プレースホルダーに文字列のコレクションを指定したログメッセージの出力を確認する() {
		// Arrange
		var logger = new TestLogger();

		// Act
		Log3(logger, new[] { "arg-1", "arg-2" });

		// Assert
		Assert.Single(logger.Messages);
		Assert.Equal("log message: arg='arg-1, arg-2'", logger.Messages.First());
	}
}
