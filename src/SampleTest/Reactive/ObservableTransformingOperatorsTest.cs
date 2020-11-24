using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Reactive {
	public class ObservableTransformingOperatorsTest {
		// http://reactivex.io/documentation/operators/scan.html
		// Aggregateに似ているが、Scanは都度onNextが呼ばれる
		[Fact]
		public void Scan_シーケンスを集計する() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Range(1, 3)
				.Scan((accumulate, current) => accumulate + current)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 1, 3, 6 }, values);
		}

		[Fact]
		public void Select_新しい形式に投影する() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Return(2)
				.Select(value => value * value)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 4 }, values);
		}

		[Fact]
		public void SelectMany_新しい形式のシーケンスに投影する() {
			// Arrange
			var actual = new List<int>();

			// Act
			Observable.Range(1, 3)
				.SelectMany(value => Observable.Range(10, value))
				.Subscribe(value => actual.Add(value));

			// Assert
			var expected = new List<int> {
				10,
				10, 11,
				10, 11, 12,
			};
			Assert.Equal(expected, actual);
		}
	}
}
