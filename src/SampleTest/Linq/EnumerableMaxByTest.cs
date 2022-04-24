using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq;

public class EnumerableMaxByTest {
	public record Sample(string Name, int Value);

	public static TheoryData<IEnumerable<Sample>, Sample?> GetTheoryDataForMaxBy() {
		return new() {
			// 最大値を持つ要素をを取得できる
			{
				// source
				new[] {
					new Sample("a", 1),
					new Sample("b", 2),
				},
				// expected
				new Sample("b", 2)
			},
			// 最大値を持つ要素が複数存在するる場合は最初の要素が見つかる
			{
				// source
				new[] {
					new Sample("a", 1),
					new Sample("b", 2),
					new Sample("c", 2),
				},
				// expected
				new Sample("b", 2)
			},
			// 空のコレクションに対しての結果はnullになる
			{
				// source
				Enumerable.Empty<Sample>(),
				// expected
				default
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForMaxBy))]
	public void MaxBy_最大値を取得する(IEnumerable<Sample> source, Sample? expected) {
		// Arrange
		// Act
		var actual = source.MaxBy(item => item.Value);

		// Assert
		Assert.Equal(expected, actual);
	}
}
