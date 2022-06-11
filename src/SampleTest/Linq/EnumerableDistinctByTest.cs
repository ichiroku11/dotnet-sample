namespace SampleTest.Linq;

public class EnumerableDistinctByTest {
	public record Sample(int Number, string Text);

	public static TheoryData<IEnumerable<Sample>, IEnumerable<Sample>> GetTheoryDataForDistinctBy() {
		return new() {
			{
				// source
				new[] { new Sample(1, "x"), new Sample(1, "x"), },
				// expected
				new[] { new Sample(1, "x"), }
			},
			{
				// source
				new[] { new Sample(1, "x"), new Sample(1, "y"), },
				// expected
				new[] { new Sample(1, "x"), new Sample(1, "y"), }
			},
			{
				// source
				new[] { new Sample(1, "x"), new Sample(2, "x"), },
				// expected
				new[] { new Sample(1, "x"), new Sample(2, "x"), }
			},
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForDistinctBy))]
	public void DistinctBy_匿名オブジェクトで重複を取り除く(IEnumerable<Sample> source, IEnumerable<Sample> expected) {
		// Arrange
		// Act
		var actual = source.DistinctBy(item => new { item.Number, item.Text });

		// Assert
		Assert.Equal(expected, actual);
	}
}
