namespace SampleTest.CSharpLanguage.OperatorsAndExpressions;

// https://learn.microsoft.com/ja-jp/dotnet/csharp/language-reference/operators/collection-expressions#spread-element
public class SpreadElementTest {
	[Fact]
	public void SpreadElement_使ってみる() {
		// Arrange
		int[] values1 = [1, 2, 3];
		int[] values2 = [4, 5];

		// Act
		int[] actual = [.. values1, .. values2];

		// 型を指定しないとエラーになるっぽい
		// CS9176
		// There is no target type for the collection expression.
		//var actual = [.. values1, .. values2];
		//var actual = [.. new[] { 1, 2, 3 }, .. new[] { 4, 5, 6 }];
		//var actual = [.. [1, 2, 3], .. [4, 5, 6]];

		// Assert
		int[] expected = [1, 2, 3, 4, 5];
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void SpreadElement_値がコピーされることを確認する() {
		// Arrange
		int[] actual = [1, 2];

		// Act
		// 値がコピーされるので変更しても
		int[] temp = [.. actual];
		temp[0] = 0;

		// Assert
		// 元の配列の値は変わらない
		int[] expected = [1, 2];
		Assert.Equal(expected, actual);
	}
}
