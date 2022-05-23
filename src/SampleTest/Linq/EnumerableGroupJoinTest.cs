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

	[Fact]
	public void GroupJoin_欠番を埋めるような動きを確認する() {
		// Arrange
		// Id=1~5までのイメージ
		var outers = Enumerable.Range(1, 5);
		// Id=2と5が欠番
		var inners = new[] {
			new { Id = 1, Value = 11 },
			new { Id = 3, Value = 13 },
			new { Id = 4, Value = 14 },
		};

		// Act
		var results = outers
			.GroupJoin(
				inners,
				outer => outer,
				inner => inner.Id,
				(outer, inners) => inners.SingleOrDefault() ?? new { Id = outer, Value = 0 })
			.OrderBy(entry => entry.Id);

		// Assert
		Assert.Equal(5, results.Count());
		Assert.Collection(results,
			result => {
				Assert.Equal(1, result.Id);
				Assert.Equal(11, result.Value);
			},
			result => {
				Assert.Equal(2, result.Id);
				Assert.Equal(0, result.Value);
			},
			result => {
				Assert.Equal(3, result.Id);
				Assert.Equal(13, result.Value);
			},
			result => {
				Assert.Equal(4, result.Id);
				Assert.Equal(14, result.Value);
			},
			result => {
				Assert.Equal(5, result.Id);
				Assert.Equal(0, result.Value);
			});
	}
}
