using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTest.Linq;

public class EnumerableLeftJoinTest {
	[Fact]
	public void LeftJoin_左外部結合の動きを確認する() {
		// Arrange
		var outers = new[] {
				new { Id = 1 },
				new { Id = 2 },
			};
		var inners = new[] {
				new { Id = 2, Value = "X" },
				new { Id = 3, Value = "Y" },
			};

		// Act
		// outers、innersそれぞれのIDで左外部結合して結果を取得する
		var results = outers
			.LeftJoin(
				inners,
				outer => outer.Id,
				inner => inner.Id,
				(outer, inner) => new { Outer = outer, Inner = inner })
			.OrderBy(entry => entry.Outer.Id);

		// Assert
		Assert.Equal(2, results.Count());
		Assert.Collection(results,
			result => {
				Assert.Equal(1, result.Outer.Id);
				Assert.Null(result.Inner);
			},
			result => {
				Assert.Equal(2, result.Outer.Id);
				Assert.NotNull(result.Inner);
				Assert.Equal("X", result.Inner.Value);
			});
	}
}
