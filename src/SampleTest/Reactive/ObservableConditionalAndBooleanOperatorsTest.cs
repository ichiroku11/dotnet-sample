using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using Xunit;

namespace SampleTest.Reactive {
	public class ObservableConditionalAndBooleanOperatorsTest {
		// http://reactivex.io/documentation/operators/all.html
		[Fact]
		public void All_発行されたすべてのアイテムが条件を満たすかどうかを判断する() {
			// Arrange
			var values = new List<bool>();

			// Act
			Observable.Range(0, 5)
				.All(value => value <= 4)
				.Subscribe(value => values.Add(value));

			// Assert
			Assert.True(values.Single());
		}

		// 含んでいるかどうか
		// http://reactivex.io/documentation/operators/contains.html
		[Fact]
		public void Any_発行された特定のアイテムが条件を満たすかどうかを判断する() {
			// Arrange
			var intercepts = new List<int>();
			var values = new List<bool>();

			// Act
			Observable.Range(0, 5)
				.Do(value => intercepts.Add(value))
				.Any(value => value == 1)
				.Subscribe(value => values.Add(value));

			// Assert
			// 条件を満たした段階で発行が終了するっぽい
			Assert.Equal(new List<int> { 0, 1 }, intercepts);
			Assert.True(values.Single());
		}
	}
}
