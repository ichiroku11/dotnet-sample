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

	[Theory]
	// ListenするActivityListenerが存在するとtrueを返す
	[InlineData(true, true)]
	// ListenしないActivityListenerが存在してもfalseを返す
	[InlineData(false, false)]
	public void HasListeners_ListenするActivityListenerが存在するとtrueを返す(bool listen, bool expected) {
		// Arrange
		using var source = new ActivitySource("test");

		// 有効なListenerとするには、ShouldListenToでtrueを返す必要がある
		using var listener = new ActivityListener {
			ShouldListenTo = _ => listen,
		};
		ActivitySource.AddActivityListener(listener);

		// Act
		// Assert
		Assert.Equal(expected, source.HasListeners());
	}

	[Fact]
	public void HasListeners_ListenするActivityListenerを破棄するとfalseを返す() {
		// Arrange
		using var source = new ActivitySource("test");

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
		};
		ActivitySource.AddActivityListener(listener);

		// 破棄する
		listener.Dispose();

		// Act
		// Assert
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

	[Theory]
	[InlineData(false)]
	[InlineData(true)]
	public void StartActivity_ListenするActivityListenerが存在しても戻り値はnull(bool samplingResultNone) {
		// Arrange
		using var source = new ActivitySource("test");
		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
		};

		if (samplingResultNone) {
			listener.Sample = (ref _) => ActivitySamplingResult.None;
		}

		ActivitySource.AddActivityListener(listener);

		Assert.True(source.HasListeners());

		// Act
		using var activity = source.StartActivity();

		// Assert
		Assert.Null(activity);
	}

	[Theory]
	[InlineData(ActivitySamplingResult.PropagationData)]
	[InlineData(ActivitySamplingResult.AllData)]
	[InlineData(ActivitySamplingResult.AllDataAndRecorded)]
	public void StartActivity_有効なActivityListenerが存在する場合はインスタンスを返す(ActivitySamplingResult samplingResult) {
		// Arrange
		using var source = new ActivitySource("test");
		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => samplingResult,
		};
		ActivitySource.AddActivityListener(listener);

		Assert.True(source.HasListeners());

		// Act
		using var activity = source.StartActivity();

		// Assert
		Assert.NotNull(activity);
	}
}
