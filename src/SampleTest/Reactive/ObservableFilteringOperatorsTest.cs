using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Reactive {
	public class ObservableFilteringOperatorsTest {
		[Fact]
		public void Where_シーケンスをフィルタする() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Range(0, 5)
				.Where(value => value % 2 == 0)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 0, 2, 4 }, values);
		}

		[Fact]
		public void Skip_シーケンスの開始から指定した数の要素を無視する() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Range(0, 5)
				.Skip(3)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 3, 4 }, values);
		}

		[Fact]
		public void Take_シーケンスの開始から指定した数の要素を取り出す() {
			// Arrange
			var values = new List<int>();

			// Act
			Observable.Range(0, 5)
				.Take(3)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.Equal(new List<int> { 0, 1, 2 }, values);
		}
	}
}
