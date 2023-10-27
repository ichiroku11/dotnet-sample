namespace SampleTest.CSharpLanguage.Operators;

// null合体演算子のテスト
public class NullCoalescingOperatorTest {
	[Fact]
	public void Null合体演算子_左辺がnullでない場合は左辺の値を返す() {
		var x = (int?)1;
		var y = -1;

		var result = x ?? y;
		// 以下と同じ
		//var result = x != null ? x : y; 

		Assert.Equal(1, result);
	}

	[Fact]
	public void Null合体演算子_左辺がnullの場合は右辺の値を返す() {
		var x = default(int?);
		var y = -1;

		var result = x ?? y;
		// 以下と同じ
		//var result = x != null ? x : y; 

		Assert.Equal(-1, result);
	}

	[Fact]
	public void Null合体演算子_左辺がnullの場合は右辺の値を返すその2() {
		var x = default(int?);
		var y = default(int?);
		var z = 0;

		var result = x ?? y ?? z;
		/*
		// 以下と同じ
		var result = x != null
			? x
			: y != null
				? y
				: z;
		*/

		Assert.Equal(0, result);
	}

	[Fact]
	public void Null合体演算子_左辺が非nullの場合に右辺が呼び出されないことを確認する() {
		// Arrange
		int? value = 1;

		// Act
		// 例外がスローされない
		int? actual = value ?? throw new InvalidOperationException();

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void Null合体演算子_評価順を確認する() {
		var count = 0;
		int? getX() {
			Assert.Equal(0, count);
			count++;
			return null;
		}

		int? getY() {
			Assert.Equal(1, count);
			count++;
			return null;
		}

		int? getZ() {
			Assert.Equal(2, count);
			count++;
			return 0;
		}

		// 前から順に評価される
		var result = getX() ?? getY() ?? getZ();

		Assert.Equal(0, result);
		Assert.Equal(3, count);
	}

	[Fact]
	public void Null合体割り当て演算子_動きを確認する() {
		int? value = null;

		value ??= 1;
		// 次のコードと同じ
		/*
		if (value is null) {
			value = 1;
		}
		*/

		Assert.Equal(1, value);
	}

	[Fact]
	public void Null合体割り当て演算子_評価した値を確認する() {
		int? value = null;

		// 割り当て後の値を返す関数
		int? getValue() => value ??= 1;

		Assert.Equal(1, getValue());
	}
}
