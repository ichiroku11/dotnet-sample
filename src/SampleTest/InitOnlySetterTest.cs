using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest;

// 参考
// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#init-only-setters
// https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
public class InitOnlySetterTest {
	public class Sample {
		public Sample(string? value = null) {
			// コンストラクタで値を設定できる
			Value = value;
		}

		// オブジェクト初期化子で値を設定できる
		public string? Value { get; init; }
	}

	[Fact]
	public void InitOnlySetter_使ってみる() {
		// Arrange
		// Act
		var sample1 = new Sample("x");
		// エラー CS8852
		// sample1.Value = "y";

		var sample2 = new Sample {
			Value = "x",
		};

		// Assert
		// 特に意味はないアサーション
		Assert.Equal("x", sample1.Value);
		Assert.Equal("x", sample2.Value);
	}
}
