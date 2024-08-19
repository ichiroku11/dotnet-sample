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
	public void Contains_DoesNotContain_文字列が含まれているか含まれていないかを検証する() {
		var text = "abcdefg";

		Assert.Contains("cde", text);
		Assert.DoesNotContain("fgh", text);
	}

	[Fact]
	public void Contains_DoesNotContain_コレクションに要素が含まれているか含まれていないかを検証する() {
		{
			// IEnumerable<int>
			var values = new[] { 1 };
			Assert.Contains(1, values);
			Assert.DoesNotContain(2, values);
		}
		{
			// IEnumerable<string>
			var values = new[] { "a" };
			Assert.Contains("a", values);
			Assert.DoesNotContain("b", values);
		}
		{
			// 条件を満たす要素が存在するか、存在しないか
			var values = new[] { 1 };
			Assert.Contains(values, value => value % 2 == 1);
			Assert.DoesNotContain(values, value => value % 2 == 0);
		}
	}
}
