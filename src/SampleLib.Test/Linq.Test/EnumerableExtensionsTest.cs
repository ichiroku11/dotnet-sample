using Xunit;

namespace SampleLib.Linq.Test;

public class EnumerableExtensionsTest {
	public static TheoryData<int[], (int, int)[]> GetTheoryData_DenseRank() {
		return new TheoryData<int[], (int, int)[]>
		{
			// 同じ順位がない
			{
				new int[] { 10, 20, 30 },
				new (int, int)[] { (10, 1), (20, 2), (30, 3) }
			},
			// 同じ順位がある
			{
				new int[] { 10, 20, 20, 30, 40 },
				new (int, int)[] { (10, 1), (20, 2), (20, 2), (30, 3), (40, 4) }
			}
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
}
