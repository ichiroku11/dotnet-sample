using System.Diagnostics;

namespace SampleTest.Diagnostics;

[Collection(CollectionNames.DotNetActivity)]
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

		Assert.Equal(ActivityTraceFlags.None, activity.ActivityTraceFlags);
		Assert.Equal(ActivityIdFormat.Unknown, activity.IdFormat);
		Assert.Equal(ActivityKind.Internal, activity.Kind);
		Assert.Equal(ActivityStatusCode.Unset, activity.Status);

		// インスタンス生成後は、Startしていないけどfalseになる
		Assert.False(activity.IsStopped);

		Assert.Null(activity.Parent);

		Assert.NotNull(activity.Source);
	}

	[Fact]
	public void Properties_ActivitySourceで生成したインスタンスのプロパティを確認する() {
		// Arrange
		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
		};
		ActivitySource.AddActivityListener(listener);

		using var source = new ActivitySource("");

		// Act
		using var activity = source.CreateActivity("test", ActivityKind.Internal);

		// Assert
		Assert.NotNull(activity);
		Assert.Equal("test", activity.OperationName);

		Assert.Null(activity.Id);
		Assert.Null(activity.ParentId);
		Assert.Null(activity.RootId);

		Assert.Equal(TimeSpan.Zero, activity.Duration);

		Assert.Empty(activity.Baggage);
		Assert.Empty(activity.Events);
		Assert.Empty(activity.Links);
		Assert.Empty(activity.Tags);

		Assert.Equal(ActivityTraceFlags.None, activity.ActivityTraceFlags);
		Assert.Equal(ActivityIdFormat.W3C, activity.IdFormat);
		Assert.Equal(ActivityKind.Internal, activity.Kind);
		Assert.Equal(ActivityStatusCode.Unset, activity.Status);

		Assert.False(activity.IsStopped);

		Assert.Null(activity.Parent);

		Assert.Same(source, activity.Source);
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
	public void DefaultIdFormat_W3Cフォーマットを返す() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(ActivityIdFormat.W3C, Activity.DefaultIdFormat);
	}

	[Fact]
	public void Baggage_親Activityから子Activityに伝播する() {
		// Arrange
		using var parent = new Activity("parent").Start();
		parent.AddBaggage("key", "value");

		// 開始する必要がある
		using var child = new Activity("child").Start();

		// Act
		var actual = child.Baggage.Single();

		// Assert
		Assert.Equal("key", actual.Key);
		Assert.Equal("value", actual.Value);
	}

	[Fact]
	public void Baggage_子ActivityをStartしないと親から子に伝播しない() {
		// Arrange
		using var parent = new Activity("parent").Start();
		parent.AddBaggage("key", "value");

		// StartしないとParentが設定されないためか
		using var child = new Activity("child");

		// Act
		// Assert
		Assert.Empty(child.Baggage);
	}

	[Fact]
	public void Events_AddExceptionで追加した例外はEventとして追加される() {
		// Arrange
		using var activity = new Activity("test");
		activity.AddException(new Exception("test-exception-message"));

		// Act
		var actual = activity.Events.Single();

		// Assert
		Assert.Equal("exception", actual.Name);
		Assert.Equal("test-exception-message", actual.Tags.First(tag => tag.Key == "exception.message").Value);
		Assert.Contains(actual.Tags, tag => tag.Key == "exception.stacktrace");
		Assert.Contains(actual.Tags, tag => tag.Key == "exception.type");
	}

	[Fact]
	public void Events_Links_Tags_親Activityから子Activityに伝播しない() {
		// Arrange
		using var parent = new Activity("parent").Start();
		parent.AddEvent(new ActivityEvent("event"));
		parent.AddLink(new ActivityLink());
		parent.AddTag("key", "value");

		// 開始する必要がある
		using var child = new Activity("child").Start();

		// Act
		// Assert
		Assert.Empty(child.Events);
		Assert.Empty(child.Links);
		Assert.Empty(child.Tags);
	}

	// 厳密に言うとStartする前からFalse
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
	public void Parent_Startしても取得できる値はnull() {
		// Arrange
		using var activity = new Activity("test").Start();

		// Act
		// Assert
		Assert.Null(activity.Parent);
	}

	[Theory]
	[InlineData(false, false)]
	[InlineData(true, false)]
	[InlineData(false, true)]
	public void Parent_親Activityと子ActivityのどちらかがStartしていない場合はnullを返す(bool startParent, bool startChild) {
		// Arrange
		using var parent = new Activity("parent");
		using var child = new Activity("child");

		if (startParent) {
			parent.Start();
		}
		if (startChild) {
			child.Start();
		}

		// Act
		// Assert
		Assert.Null(child.Parent);
	}

	[Fact]
	public void Parent_入れ子にしてStartすると親のインスタンスを取得できる() {
		// Arrange
		using var parent = new Activity("parent").Start();
		using var child = new Activity("child").Start();

		// Act
		// Assert
		Assert.Same(parent, child.Parent);
	}

	[Fact]
	public void Parent_ActivitySourceで生成したインスタンスを入れ子にすると親インスタンスを取得できる() {
		// Arrange
		using var source = new ActivitySource("test");

		using var listener = new ActivityListener {
			ShouldListenTo = _ => true,
			Sample = (ref _) => ActivitySamplingResult.PropagationData,
		};
		ActivitySource.AddActivityListener(listener);

		// Act
		using var parent = source.StartActivity();
		using var child = source.StartActivity();

		// Assert
		Assert.NotNull(parent);
		Assert.NotNull(child);
		Assert.Same(parent, child.Parent);
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

	[Fact]
	public void Source_インスタンスを複数生成した場合のSourceは同じインスタンス() {
		// Arrange
		using var activity1 = new Activity("test1");
		using var activity2 = new Activity("test2");

		// Act
		// Assert
		Assert.Same(activity1.Source, activity2.Source);
	}
}
