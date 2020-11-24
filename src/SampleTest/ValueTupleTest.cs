using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SampleTest {
	// 分解の確認用
	public struct Vector3 {
		public Vector3(int x = 0, int y = 0, int z = 0) {
			X = x;
			Y = y;
			Z = z;
		}
		// コンストラクタは次のようにも書けるみたい
		/*
		public Vector3(int x = 0, int y = 0, int z = 0) => (X, Y, Z) = (x, y, z);
		*/

		public int X { get; }
		public int Y { get; }
		public int Z { get; }

		// 分解メソッド
		public void Deconstruct(out int x, out int y, out int z) => (x, y, z) = (X, Y, Z);
	}

	public static class Vector3Extensions {
		// 拡張メソッドによる分解メソッド
		public static void Deconstruct(this Vector3 vector, out int x, out int y) => (x, y, _) = vector;
	}

	public class ValueTupleTest {
		[Fact(DisplayName = "ValueTuple_名前がないタプルはItem1、Item2といったフィルード名でアクセスできる")]
		public void ValueTuple_Unnamed() {
			var unnamed = ("a", 1);

			Assert.Equal("a", unnamed.Item1);
			Assert.Equal(1, unnamed.Item2);
		}

		[Fact(DisplayName = "ValueTuple_名前付きタプルは指定したフィールド名でアクセスできる")]
		public void ValueTuple_Named() {
			var named = (first: "a", second: 1);

			Assert.Equal("a", named.first);
			Assert.Equal(1, named.second);

			// Item1、Item2といったフィールド名でもアクセスできる
			Assert.Equal("a", named.Item1);
			Assert.Equal(1, named.Item2);
		}

		[Fact]
		public void ValueTuple_タプルのプロジェクション初期化子() {
			var x = 1;
			var y = 2;

			// プロジェクション初期化子
			// 暗黙的な名前が射影されるというやつ
			var tuple = (x, y);

			Assert.Equal(x, tuple.x);
			Assert.Equal(y, tuple.y);
		}

		[Fact]
		public void ValueTuple_タプルは比較できる() {
			var left = (x: 1, y: 2);
			var right = (x: 1, y: 2);

			Assert.True(left == right);
		}

		[Fact]
		public void ValueTuple_タプルをメソッドの戻り値として使う() {
			// タプルを返すメソッド
			static (int min, int max) getRange() => (1, 3);

			var range = getRange();
			Assert.Equal(1, range.min);
			Assert.Equal(3, range.max);

			// タプルを分解する（たぶんこっちの方がよい？）
			var (min, max) = getRange();
			// 以下の書き方も可
			//(int min, int max) = getRange();
			//(var min, var max) = getRange();
			Assert.Equal(1, min);
			Assert.Equal(3, max);
		}

		[Fact]
		public void ValueTuple_タプルをLINQで使う() {
			static IEnumerable<(int value, int index)> getItems()
				=> Enumerable.Range(1, 3).Select((value, index) => (value, index));

			var items = getItems();
			Assert.Equal((1, 0), items.First());
			Assert.Equal((2, 1), items.Skip(1).First());
			Assert.Equal((3, 2), items.Last());
		}

		[Fact]
		public void Deconstruct_ユーザ定義型の分解を試す() {
			var vector = new Vector3(1, 2, 3);

			var (x, y, z) = vector;

			Assert.Equal(1, x);
			Assert.Equal(2, y);
			Assert.Equal(3, z);
		}

		[Fact]
		public void Deconstruct_ユーザ定義型の拡張メソッドによる分解を試す() {
			var vector = new Vector3(1, 2, 3);

			var (x, y) = vector;

			Assert.Equal(1, x);
			Assert.Equal(2, y);
		}
	}
}
