using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using Xunit;

namespace SampleTest.Reactive {
	public class ObservableCombiningOperatorsTest {
		// CombineLatestはZipに近いかも
		[Fact]
		public void CombineLatest_シーケンスの最新を結合する() {
			// Arrange
			var subject1 = new Subject<int>();
			var subject2 = new Subject<int>();
			var values = new List<string>();

			// Act
			// Assert
			subject1
				.CombineLatest(subject2, (value1, value2) => $"{value1}:{value2}")
				.Subscribe(value => values.Add(value));

			// 2つ目のシーケンスが発行されていないのでまだ空
			subject1.OnNext(1);
			Assert.Empty(values);

			// 2つ目のシーケンスが発行されたので発行される
			subject2.OnNext(2);
			Assert.Equal(new List<string> { "1:2" }, values);

			subject2.OnNext(3);
			Assert.Equal(new List<string> { "1:2", "1:3" }, values);

			subject1.OnNext(4);
			Assert.Equal(new List<string> { "1:2", "1:3", "4:3" }, values);
		}

		[Fact]
		public void Merge_シーケンスをマージする() {
			// Arrange
			var subject1 = new Subject<int>();
			var subject2 = new Subject<int>();
			var values = new List<int>();

			// Act
			Observable.Merge(subject1, subject2)
				.Subscribe(value => values.Add(value));
			// 以下でも同じこと
			/*
			subject1.Merge(subject2)
				.Subscribe(value => values.Add(value));
			*/

			// マージ元から発行されたら即座に後続に発行
			subject1.OnNext(1);
			subject2.OnNext(2);
			subject1.OnNext(3);
			subject2.OnNext(4);

			// Assert
			Assert.Equal(new List<int> { 1, 2, 3, 4 }, values);

			subject1.Dispose();
			subject2.Dispose();
		}

		[Fact]
		public void StartWith_シーケンスの前に発行する() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Return(3)
				.StartWith(1, 2)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 1, 2, 3 }, values);
		}

		[Fact]
		public void Zip_シーケンスをマージする() {
			// Arrange
			var values = new List<string>();

			// Act
			Observable.Range(1, 3)
				.Zip(
					Observable.Range(4, 3),
					(value1, value2) => $"{value1}:{value2}")
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<string> { "1:4", "2:5", "3:6" }, values);
		}

		[Fact]
		public void Zip_どちらかのシーケンスが完了した段階で完了する() {
			// Arrange
			var values = new List<string>();

			// Act
			// 1つ目はシーケンスは3つ発行して完了
			// 2つ目のシーケンスは2つ発行して完了
			Observable.Range(1, 3)
				.Zip(
					Observable.Range(4, 2),
					(value1, value2) => $"{value1}:{value2}")
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<string> { "1:4", "2:5" }, values);
		}
	}
}
