namespace SampleTest.Linq;

public class EnumerableAggregateByTest {
	[Fact]
	public void AggregateBy_グループ別に集計する() {
		// Arrange
		var values = "abacbb".ToCharArray();

		// Act
		var actual = values.AggregateBy(
			keySelector: value => value,
			seed: "",
			func: (accumlated, value) => accumlated + value);

		// Assert
		Assert.Equal(3, actual.Count());
		Assert.Single(actual, item => item.Key == 'a' && item.Value == "aa");
		Assert.Single(actual, item => item.Key == 'b' && item.Value == "bbb");
		Assert.Single(actual, item => item.Key == 'c' && item.Value == "c");
	}
}
