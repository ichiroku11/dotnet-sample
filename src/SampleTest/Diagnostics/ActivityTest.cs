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

		Assert.Empty(activity.Baggage);
		Assert.Empty(activity.Events);
		Assert.Empty(activity.Links);
		Assert.Empty(activity.Tags);

		Assert.Equal(ActivityKind.Internal, activity.Kind);
		Assert.Equal(ActivityStatusCode.Unset, activity.Status);
		Assert.Equal(ActivityTraceFlags.None, activity.ActivityTraceFlags);

		// インスタンス生成後は、Startしていないけどfalseになる
		Assert.False(activity.IsStopped);
	}

	[Fact]
	public void Current_取得できる値はnull() {
		// Arrange

		// Act
		// Assert
		Assert.Null(Activity.Current);
	}

	[Fact]
	public void Current_インスタンスを生成しても取得できる値はnull() {
		// Arrange
		using var activity = new Activity("test");

		// Act
		// Assert
		Assert.Null(Activity.Current);
	}

	[Fact]
	public void Current_インスタンスを生成しStartするとそのインスタンスの値を取得できる() {
		// Arrange
		using var activity = new Activity("test").Start();

		// Act
		// Assert
		Assert.Same(activity, Activity.Current);
	}

	[Fact]
	public void Current_インスタンスをStopするとnullになる() {
		// Arrange
		using var activity = new Activity("test").Start();
		activity.Stop();

		// Act
		// Assert
		Assert.Null(Activity.Current);
	}

	[Fact]
	public void Current_インスタンスをDisposeするとnullになる() {
		// Arrange
		using var activity = new Activity("test").Start();
		activity.Dispose();

		// Act
		// Assert
		Assert.Null(Activity.Current);
	}

	[Fact]
	public void IsStopped_Startするとfalse() {
		// Arrange
		using var activity = new Activity("test").Start();

		// Act
		// Assert
		Assert.False(activity.IsStopped);
	}

	[Fact]
	public void IsStopped_StartしてもStopするとtrue() {
		// Arrange
		using var activity = new Activity("test").Start();
		activity.Stop();

		// Act
		// Assert
		Assert.True(activity.IsStopped);
	}

	[Fact]
	public void Start_戻り値はメソッドを呼び出したインスタンス自身() {
		// Arrange
		using var activity = new Activity("test");

		// Act
		var started = activity.Start();

		// Assert
		Assert.Same(activity, started);
	}
}
