using Xunit;

namespace SampleLib.Linq.Test;

public class EnumerableExtensionsTest {
	public static TheoryData<int[], (int, int)[]> GetTheoryData_DenseRank() {
		return new TheoryData<int[], (int, int)[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new (int, int)[] { (10, 1), (20, 2), (30, 3) }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 20, 30, 40 },
				new (int, int)[] { (10, 1), (20, 2), (20, 2), (30, 3), (40, 4) }
			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 20, 10, 20 },
				new (int, int)[] { (30, 3), (40, 4), (20, 2), (10, 1), (20, 2) }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_DenseRank))]
	public void DenseRank(int[] values, (int, int)[] expected) {
		// Arrange

		// Act
		var actual = values.DenseRank();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<int[], (int, int)[]> GetTheoryData_DenseRankDescending() {
		return new TheoryData<int[], (int, int)[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new (int, int)[] { (10, 3), (20, 2), (30, 1) }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 30, 30, 40 },
				new (int, int)[] { (10, 4), (20, 3), (30, 2), (30, 2), (40, 1) }
			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 30, 10, 20 },
				new (int, int)[] { (30, 2), (40, 1), (30, 2), (10, 5), (20, 4) }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_DenseRankDescending))]
	public void DenseRankDescending(int[] values, (int, int)[] expected) {
		// Arrange

		// Act
		var actual = values.DenseRankDescending();

		// Assert
		Assert.Equal(expected, actual);
	}
}
