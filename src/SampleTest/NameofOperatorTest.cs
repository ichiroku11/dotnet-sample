using Xunit;

namespace SampleTest;

public class NameofOperatorTest {
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/nameof
	[Fact]
	public void NameofOperator_名前空間と型とメンバーの名前を確認する() {
		// 名前空間：完全修飾ではない
		Assert.Equal("Generic", nameof(System.Collections.Generic));

		// 型：完全修飾ではない
		Assert.Equal("DateTime", nameof(DateTime));
		// "System."は不要
		//Assert.Equal("DateTime", nameof(System.DateTime));

		// 型：ジェネリクスの場合、型引数は取得できない
		Assert.Equal("List", nameof(List<int>));

		// メソッド、プロパティ：型名は取得できない
		Assert.Equal("Add", nameof(List<int>.Add));
		Assert.Equal("Count", nameof(List<int>.Count));
	}

	[Fact]
	public void NameofOperator_変数とメンバーの名前を確認する() {
		var numbers = new List<int> { 1, 2, 3, };

		// 変数
		Assert.Equal("numbers", nameof(numbers));

		// 変数からメソッド名、プロパティ名も取得できる
		Assert.Equal("Add", nameof(numbers.Add));
		Assert.Equal("Count", nameof(numbers.Count));
	}
}
