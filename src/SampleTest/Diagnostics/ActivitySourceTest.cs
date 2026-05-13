using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class ActivitySourceTest {
	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		using var source = new ActivitySource("test");

		// Act
		// Assert
		Assert.Equal("test", source.Name);
		Assert.Equal("", source.Version);
		Assert.Null(source.Tags);
		Assert.Null(source.TelemetrySchemaUrl);
		Assert.False(source.HasListeners());
	}
}
