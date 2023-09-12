namespace SampleTest.xUnit;

public class AssertTest {
	[Fact]
	public void All_すべての要素がパスすることを確認する() {
		Assert.All(new[] { 2, 4, 6 }, value => Assert.Equal(0, value % 2));
	}
}
