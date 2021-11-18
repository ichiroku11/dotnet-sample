using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq;

public class EnumerableAnyTest {
	private static IEnumerable<int> Range(int start, int count, Action<int> action) {
		for (var index = start; index < count; index++) {
			action(index);
			yield return index;
		}
	}

	[Fact]
	public void Any_シーケンスの列挙を途中で中止する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any();

		// Assert
		Assert.True(any);
		Assert.Equal(new List<int> { 0 }, values);
	}

	[Fact]
	public void Any_条件を満たす要素が見つかればシーケンスの列挙を途中で中止する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any(value => value >= 2);

		// Assert
		Assert.True(any);
		Assert.Equal(new List<int> { 0, 1, 2 }, values);
	}

	[Fact]
	public void Any_条件を満たす要素が見つからないのでシーケンスを最後まで列挙する() {
		// Arrange
		var values = new List<int>();

		// Act
		var any = Range(0, 5, value => values.Add(value)).Any(value => value == -1);

		// Assert
		Assert.False(any);
		Assert.Equal(new List<int> { 0, 1, 2, 3, 4 }, values);
	}

}
