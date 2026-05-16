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

	[Fact]
	public void Properties_Activityインスタンスを生成した場合のSourceを確認する() {
		// Arrange
		using var activity = new Activity("test");
		var source = activity.Source;

		// Act
		// Assert
		Assert.NotNull(source);
		Assert.Equal("", source.Name);
		Assert.Equal("", source.Version);
		Assert.Null(source.Tags);
		Assert.Null(source.TelemetrySchemaUrl);
		Assert.False(source.HasListeners());
	}

	[Fact]
	public void StartActivity_戻り値はnull() {
		// Arrange
		using var source = new ActivitySource("test");

		// Act
		using var activity = source.StartActivity();

		// Assert
		Assert.Null(activity);
	}
}
