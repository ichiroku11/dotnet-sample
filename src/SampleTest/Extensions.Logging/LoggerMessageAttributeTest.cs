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
}
