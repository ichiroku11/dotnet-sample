using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest {
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
	}
}
