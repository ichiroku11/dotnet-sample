namespace SampleTest.xUnit;

public class AssertTest {
	[Fact]
	public void All_すべての要素がパスすることを確認する() {
		Assert.All(new[] { 2, 4, 6 }, value => Assert.Equal(0, value % 2));
	}

	[Fact]
	public void NotEqual_文字列の比較は大文字小文字を区別する() {
		Assert.NotEqual("x", "X");
	}

	[Fact]
	public async Task ThrowsAsync_例外の発生をテストする() {
		var exception = await Assert.ThrowsAsync<InvalidOperationException>(() => {
			throw new InvalidOperationException();
		});
	}
}
