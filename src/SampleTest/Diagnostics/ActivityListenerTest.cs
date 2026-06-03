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
				// 呼び出しは1回だけのはず
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

	// todo:
	// ActivityStarted
	// ActivityStopped
	// ExceptionRecorder
}
