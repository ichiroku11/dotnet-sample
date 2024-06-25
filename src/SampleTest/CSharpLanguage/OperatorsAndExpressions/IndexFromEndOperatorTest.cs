namespace SampleTest.CSharpLanguage.OperatorsAndExpressions;

// 参考
// https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/member-access-operators#index-from-end-operator-
public class IndexFromEndOperatorTest {
	[Fact]
	public void IndexFromEnd_試してみる() {
		var numbers = "0123456789".ToCharArray();
		// 0番目
		Assert.Equal('0', numbers[0]);

		// numbers.Length-1番目
		Assert.Equal('9', numbers[^1]);
	}

	[Fact]
	public void IndexFromEnd_例外をスローする() {
		var numbers = "0123456789".ToCharArray();

		// ^0はnumbers.Lengthのことであり例外
		Assert.Throws<IndexOutOfRangeException>(() => {
			var never = numbers[^0];
		});
	}
}
