using System.Reactive.Subjects;

namespace SampleTest.Reactive;

public class SubjectTest {
	[Fact]
	public void Subjectを使ってみる() {
		// Arrange
		var subject = new Subject<int>();
		var values = new List<int>();
		var completed = false;

		subject.Subscribe(
			onNext: value => values.Add(value),
			onError: _ => Assert.Fail(),
			onCompleted: () => completed = true);

		// Act
		// Assert
		subject.OnNext(1);
		Assert.Equal([1], values);
		Assert.False(completed);

		subject.OnNext(2);
		Assert.Equal([1, 2], values);
		Assert.False(completed);

		// onCompletedが呼ばれる
		subject.OnCompleted();
		Assert.Equal([1, 2], values);
		Assert.True(completed);

		// OnCompletedした後、onNextは呼ばれない
		subject.OnNext(4);
		Assert.Equal([1, 2], values);
		Assert.True(completed);
	}
}
