using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq;

public class EnumerableMaxByTest {
	public record Sample(string Name, int Value);

	public static IEnumerable<object?[]> GetTestDataForMaxBy() {
		// 最大値を持つ要素をを取得できる
		yield return new object[] {
			// source
			new[] {
				new Sample("a", 1),
				new Sample("b", 2),
			},
			// expected
			new Sample("b", 2),
		};

		// 空のコレクションに対しての結果はnullになる
		yield return new object?[] {
			// source
			Enumerable.Empty<Sample>(),
			// expected
			default(Sample),
		};
	}

	[Theory]
	[MemberData(nameof(GetTestDataForMaxBy))]
	public void MaxBy_最大値を取得する(IEnumerable<Sample> source, Sample? expected) {
		// Arrange
		// Act
		var actual = source.MaxBy(item => item.Value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
