using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Xunit;

namespace SampleTest.Reactive {
	public class ObservableMathematicalAndAggregateOperatorsTest {
		// http://reactivex.io/documentation/operators/reduce.html
		// Scanに似ている
		[Fact]
		public void Aggregate_シーケンスを集計する() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Range(1, 3)
				.Aggregate((accumulate, current) => accumulate + current)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 6 }, values);
		}

		[Fact]
		public void Concat_シーケンスを連結する() {
			// Arrange
			var subject1 = new Subject<int>();
			var subject2 = new Subject<int>();
			var values = new List<int>();

			// Act
			// Assert
			Observable.Concat(subject1, subject2)
				.Subscribe(value => values.Add(value));
			/*
			// 以下でも同じこと
			subject1.Concat(subject2)
				.Subscribe(value => values.Add(value));
			*/

			// 1つ目が完了するまでは、2つ目のシーケンスは発行されない
			subject1.OnNext(1);
			subject2.OnNext(2);
			Assert.Equal(new List<int> { 1 }, values);

			// 1つ目が完了しても、それまでに発行された2つ目のシーケンスは発行されない
			// 「2」は発行されない
			subject1.OnCompleted();
			Assert.Equal(new List<int> { 1 }, values);

			// 2つ目のシーケンスが発行される
			subject2.OnNext(3);
			subject2.OnNext(4);
			Assert.Equal(new List<int> { 1, 3, 4 }, values);
		}
	}
}
