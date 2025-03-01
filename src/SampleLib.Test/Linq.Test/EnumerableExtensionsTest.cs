using Xunit;

namespace SampleLib.Linq.Test;

public class EnumerableExtensionsTest {
	public static TheoryData<int[], int[]> GetTheoryData_DenseRank() {
		return new TheoryData<int[], int[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new int[] { 3, 2, 1 }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 30, 30, 40 },
				new int[] { 4, 3, 2, 2, 1 }

			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 30, 10, 20 },
				new int[] { 2, 1, 2, 4, 3 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_DenseRank))]
	public void DenseRank(int[] values, int[] rank) {
		// Arrange
		var expected = values.Zip(rank);

		// Act
		var actual = values.DenseRank();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<int[], int[]> GetTheoryData_DenseRankDescending() {
		return new TheoryData<int[], int[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new int[] { 1, 2, 3 }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 20, 30, 40 },
				new int[] { 1, 2, 2, 3, 4 }
			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 20, 10, 20 },
				new int[] { 3, 4, 2, 1, 2 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_DenseRankDescending))]
	public void DenseRankDescending(int[] values, int[] rank) {
		// Arrange
		var expected = values.Zip(rank);

		// Act
		var actual = values.DenseRankDescending();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<int[], int[]> GetTheoryData_Rank() {
		return new TheoryData<int[], int[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new int[] { 3, 2, 1 }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 30, 30, 40 },
				new int[] { 5, 4, 2, 2, 1 }
			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 30, 10, 20 },
				new int[] { 2, 1, 2, 5, 4 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Rank))]
	public void Rank(int[] values, int[] rank) {
		// Arrange
		var expected = values.Zip(rank);

		// Act
		var actual = values.Rank();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<int[], int[]> GetTheoryData_RankDescending() {
		return new TheoryData<int[], int[]> {
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new int[] { 1, 2, 3 }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 20, 30, 40 },
				new int[] { 1, 2, 2, 4, 5 }

			},
			// 同じ順位があり、シーケンスの並び順は適当
			// => シーケンスの並びは変わらない
			{
				new int[] { 30, 40, 20, 10, 20 },
				new int[] { 4, 5, 2, 1, 2 }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_RankDescending))]
	public void RankDescending(int[] values, int[] rank) {
		// Arrange
		var expected = values.Zip(rank);

		// Act
		var actual = values.RankDescending();

		// Assert
		Assert.Equal(expected, actual);
	}
}
