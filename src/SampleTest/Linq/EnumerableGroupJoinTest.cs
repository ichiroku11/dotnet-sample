using Xunit;

namespace SampleTest.Linq;

public class EnumerableGroupJoinTest {
	[Fact]
	public void GroupJoin_左外部結合のような動きを確認する() {
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
			.GroupJoin(
				inners,
				outer => outer.Id,
				inner => inner.Id,
				(outer, inners) => new { outer.Id, Inners = inners })
			.OrderBy(entry => entry.Id);

		// Assert
		Assert.Equal(2, results.Count());
		Assert.Collection(results,
			result => {
				Assert.Equal(1, result.Id);
				Assert.Empty(result.Inners);
			},
			result => {
				Assert.Equal(2, result.Id);
				Assert.Single(result.Inners);
				Assert.Equal("X", result.Inners.First().Value);
			});
	}
}
