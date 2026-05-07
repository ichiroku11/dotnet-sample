using System.Diagnostics;

namespace SampleTest.Diagnostics;

public class ActivityTest {
	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		using var activity = new Activity("test");

		// Assert
		Assert.Equal("test", activity.OperationName);

		Assert.Null(activity.Id);
		Assert.Null(activity.ParentId);
		Assert.Null(activity.RootId);

		Assert.Equal(TimeSpan.Zero, activity.Duration);

		Assert.Empty(activity.Tags);
		Assert.Empty(activity.Events);

		Assert.Equal(ActivityKind.Internal, activity.Kind);
		Assert.Equal(ActivityStatusCode.Unset, activity.Status);
		Assert.Equal(ActivityTraceFlags.None, activity.ActivityTraceFlags);
	}
}
