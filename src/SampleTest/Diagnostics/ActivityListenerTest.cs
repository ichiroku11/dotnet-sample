using System.Diagnostics;

namespace SampleTest.Diagnostics;

[Collection(CollectionNames.DotNetActivity)]
public class ActivityListenerTest {
	// ShouldListenTo
	// ActivitySource.AddActivityListenerやnew ActivitySourceのタイミングで発生する様子
	// ActivityListenerはstatic変数で管理されているようで、
	// 呼ばれる頻度が高く、他のコードの影響を受けるのでテストが難しかった

	[Fact]
	public void Sample_Activityを生成したときに呼び出される() {
		// Arrange
		var sampled = false;

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref options) => {
				// このテストでは2回呼ばれない
				Assert.False(sampled);

				sampled = true;

				return ActivitySamplingResult.None;
			},
		};
		ActivitySource.AddActivityListener(listener);

		using var source = new ActivitySource("");

		// Act
		// Assert
		Assert.False(sampled);

		using var activity = source.CreateActivity("test", ActivityKind.Internal);
		Assert.True(sampled);

		Assert.Null(activity);
	}

	[Fact]
	public void ActivityStarted_ActivitySourceのStartActivityで開始したときに呼び出される() {
		// Arrange
		var started = false;
		var startedActivity = default(Activity);

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
			ActivityStarted = activity => {
				// このテストでは2回呼ばれない
				Assert.False(started);

				started = true;
				startedActivity = activity;
			},
		};
		ActivitySource.AddActivityListener(listener);

		using var source = new ActivitySource("");

		// Act
		// Assert
		Assert.False(started);

		using var activity = source.StartActivity();
		Assert.True(started);

		Assert.Same(activity, startedActivity);
	}

	[Fact]
	public void ActivityStarted_ActivityをStartしたときに呼び出される() {
		// Arrange
		var started = false;
		var startedActivity = default(Activity);

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
			ActivityStarted = activity => {
				// このテストでは2回呼ばれない
				Assert.False(started);

				started = true;
				startedActivity = activity;
			},
		};
		ActivitySource.AddActivityListener(listener);

		// Act
		// Assert
		Assert.False(started);

		using var activity = new Activity("test");
		Assert.False(started);

		activity.Start();
		Assert.True(started);

		Assert.Same(activity, startedActivity);
	}

	[Fact]
	public void ActivityStopped_ActivityをStopしたときに呼び出される() {
		// Arrange
		var stopped = false;
		var stoppedActivity = default(Activity);

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
			ActivityStopped = activity => {
				// このテストでは2回呼ばれない
				Assert.False(stopped);

				stopped = true;
				stoppedActivity = activity;
			},
		};
		ActivitySource.AddActivityListener(listener);

		using var activity = new Activity("test").Start();

		// Act
		// Assert
		Assert.False(stopped);

		activity.Stop();
		Assert.True(stopped);

		Assert.Same(activity, stoppedActivity);
	}

	[Fact]
	public void ActivityStopped_ActivityをDisposeしたときに呼び出される() {
		// Arrange
		var stopped = false;
		var stoppedActivity = default(Activity);

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
			ActivityStopped = activity => {
				// このテストでは2回呼ばれない
				Assert.False(stopped);

				stopped = true;
				stoppedActivity = activity;
			},
		};
		ActivitySource.AddActivityListener(listener);

		using var activity = new Activity("test").Start();

		// Act
		// Assert
		Assert.False(stopped);

		activity.Dispose();
		Assert.True(stopped);

		Assert.Same(activity, stoppedActivity);
	}

	// todo:
	// ExceptionRecorder
}
