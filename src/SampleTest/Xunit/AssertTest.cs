namespace SampleTest.Xunit;

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

	[Fact]
	public void Contains_色々な使い方() {
		// 文字列が含まれているか
		Assert.Contains("cde", "abcdefg");

		// 要素がコレクションに含まれているか
		Assert.Contains(1, new[] { 1, 2, 3 });
		Assert.Contains("a", new[] { "a", "b", "c" });

		// コレクションの要素が条件を満たすか
		Assert.Contains(new[] { 1, 2, 3 }, value => value == 2);
	}
}
