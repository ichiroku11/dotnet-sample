namespace SampleTest.Collections;

public class ArrayTest {
	// 参考
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-8#indices-and-ranges
	[Fact]
	public void Index_試してみる() {
		var numbers = "0123456789".ToCharArray();
		// 0番目
		Assert.Equal('0', numbers[0]);

		// numbers.Length-1番目
		Assert.Equal('9', numbers[^1]);
	}

	[Fact]
	public void Index_例外をスローする() {
		var numbers = "0123456789".ToCharArray();

		// ^0はnumbers.Lengthのことであり例外
		Assert.Throws<IndexOutOfRangeException>(() => {
			var never = numbers[^0];
		});
	}

	[Fact]
	public void Range_試してみる() {
		var numbers = "0123456789".ToCharArray();

		// すべて
		Assert.Equal("0123456789".ToCharArray(), numbers[..]);

		// 1番目からnumbers.Length-1番目
		Assert.Equal("12345678".ToCharArray(), numbers[1..^1]);

		// 最後の2つ
		Assert.Equal("89".ToCharArray(), numbers[^2..]);

		// 変数で範囲を宣言する
		var range = 2..4;
		Assert.Equal("23".ToCharArray(), numbers[range]);
	}
}
