using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest {
	// 参考
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#init-only-setters
	// https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
	public class InitOnlySetterTest {
		public class Sample {
			public string Value { get; init; }
		}

		[Fact]
		public void InitOnlySetter_使ってみる() {
			// Arrange
			// Act
			// Assert

			// オブジェクト初期化子で値を設定できる
			var sample = new Sample {
				Value = "X",
			};

			// エラー CS8852
			// sample.Value = "y";

			// Assert
			// 特に意味はない
			Assert.Equal("x", sample.Value);
		}
	}
}
