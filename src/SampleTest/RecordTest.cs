using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest {
	// 参考
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#record-types
	public class RecordTest {
		// レコード型
		// コンストラクタの引数の名前は大文字で始める必要あり？
		record Vector2(int X = 0, int Y = 0);

		[Fact]
		public void Record_とりあえず使ってみる() {
			// Arrange
			// Act
			var vector = new Vector2(1, 2);

			// Assert
			// コンストラクタ引数の名前のプロパティができる
			Assert.Equal(1, vector.X);
			Assert.Equal(2, vector.Y);

			// コンパイルエラー
			//vector.X = 1;
		}

		[Fact]
		public void Record_オブジェクト初期化子を使ってインスタンス化する() {
			// Arrange
			// Act
			var vector = new Vector2 {
				X = 1,
				Y = 2,
			};

			// Assert
			Assert.Equal(1, vector.X);
			Assert.Equal(2, vector.Y);
		}

		[Fact]
		public void Deconstruct_プロパティを分解できる() {
			// Arrange
			// Act
			var (x, y) = new Vector2 {
				X = 1,
				Y = 2,
			};

			// Assert
			Assert.Equal(1, x);
			Assert.Equal(2, y);
		}

		[Fact]
		public void Equals_参照の比較ではなく値の比較になる() {
			// Arrange
			// Act
			var vector1 = new Vector2 {
				X = 1,
				Y = 2,
			};
			var vector2 = new Vector2 {
				X = 1,
				Y = 2,
			};

			// Assert
			Assert.True(vector1.Equals(vector2));
			Assert.True(vector1 == vector2);
			Assert.False(object.ReferenceEquals(vector1, vector2));
		}

		[Fact]
		public void ToString_出力される文字列を確認する() {
			// Arrange
			var vector = new Vector2 {
				X = 1,
				Y = 2,
			};

			// Act
			// Assert
			Assert.Equal("Vector2 { X = 1, Y = 2 }", vector.ToString());
		}

		[Fact]
		public void With_with式でコピーできる() {
			// Arrange
			var vector1 = new Vector2 {
				X = 1,
				Y = 2,
			};

			// Act
			var vector2 = vector1 with {
				X = 3,
			};

			// Assert
			Assert.Equal(1, vector1.X);
			Assert.Equal(3, vector2.X);
		}
	}
}
