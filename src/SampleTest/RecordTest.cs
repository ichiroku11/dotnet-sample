using Xunit;

namespace SampleTest;

// 参考
// https://docs.microsoft.com/ja-jp/dotnet/csharp/whats-new/csharp-9#record-types
// https://docs.microsoft.com/ja-jp/dotnet/csharp/tutorials/exploration/records
// https://devblogs.microsoft.com/dotnet/c-9-0-on-the-record/
public class RecordTest {
	// 最小のレコード型
	// 型名の直後()で引数を並べる書き方をプライマリコンストラクタと言う
	// コンストラクタの引数の名前は大文字で始める必要あり？
	private record Vector2(int X = 0, int Y = 0);
	// 以下のメソッドが実装される
	// Equals
	// GetHashCode
	// operator ==
	// operator !=
	// IEquatable<T>

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

	// record型にメソッドを追加できる
	private record Sample1(int Value = 0) {
		// 特に意味のないメソッド
		public int GetValue() => Value;
	}

	[Fact]
	public void Record_メソッドを追加できる() {
		// Arrange
		// Act
		var sample = new Sample1 {
			Value = 1
		};

		// Assert
		Assert.Equal(1, sample.Value);
		Assert.Equal(1, sample.GetValue());
	}

	// レコード型の継承を試す
	private record Base(int Value);
	// ベースの型のプライマリコンストラクタを呼び出す必要がある
	// 同じプロパティ名を指定できる
	private record Derived(int Value) : Base(Value);

	[Fact]
	public void Record_継承を試す() {
		// Arrange
		// Act
		var d = new Derived(1);

		// Assert
		Assert.Equal(1, d.Value);
	}

	[Fact]
	public void Record_同じ値を持っていても型は異なる() {
		// Arrange
		// Act
		var b = new Base(1);
		var d = new Derived(1);

		// Assert
		Assert.False(b == d);
	}
}
