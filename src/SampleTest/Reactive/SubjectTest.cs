using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;
using Xunit;

namespace SampleTest.Reactive {
	public class SubjectTest {
		[Fact]
		public void Subjectを使ってみる() {
			// Arrange
			var subject = new Subject<int>();
			var values = new List<int>();
			var completed = false;

			subject.Subscribe(
				onNext: value => values.Add(value),
				onError: _ => AssertHelper.Fail(),
				onCompleted: () => completed = true);

			// Act
			// Assert
			subject.OnNext(1);
			Assert.Equal(new List<int> { 1 }, values);
			Assert.False(completed);

			subject.OnNext(2);
			Assert.Equal(new List<int> { 1, 2 }, values);
			Assert.False(completed);

			// onCompletedが呼ばれる
			subject.OnCompleted();
			Assert.Equal(new List<int> { 1, 2 }, values);
			Assert.True(completed);

			// OnCompletedした後、onNextは呼ばれない
			subject.OnNext(4);
			Assert.Equal(new List<int> { 1, 2 }, values);
			Assert.True(completed);
		}
	}
}
