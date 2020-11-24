using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	// Moqの使い方
	// 参考
	// https://qiita.com/usamik26/items/42959d8b95397d3a8ffb
	public class MoqTest {
		// テスト対象
		public interface ITestTarget<TValue> {
			TValue SomeValue { get; set; }
			TValue GetSomeValue(TValue param);
		}

		[Fact]
		public void Setup_固定値を返すプロパティをエミュレートする() {
			var mock = new Mock<ITestTarget<int>>();

			// 固定値を返すプロパティ
			mock.Setup(target => target.SomeValue)
				.Returns(1);

			// セットアップした値を取得できる
			Assert.Equal(1, mock.Object.SomeValue);
		}

		[Fact]
		public void Setup_固定値を返すプロパティに値をセットしても変更されない() {
			var mock = new Mock<ITestTarget<int>>();

			// 固定値を返すプロパティ
			mock.Setup(target => target.SomeValue)
				.Returns(1);

			// プロパティの値を変更する
			mock.Object.SomeValue = 2;

			// 取得できるのはセットアップした値
			Assert.Equal(1, mock.Object.SomeValue);
		}

		[Fact]
		public void Setup_コレクションのプロパティにデータを挿入したり削除したり() {
			var mock = new Mock<ITestTarget<IList<int>>>();

			// コレクションのプロパティ
			var expected = new List<int> { 1 };
			mock.Setup(target => target.SomeValue)
				.Returns(expected);

			var actual = mock.Object.SomeValue;

			// Returnsで指定したインスタンスを取得できる
			Assert.Same(expected, actual);

			// 初期値が含まれている
			Assert.Equal(1, actual.Count);
			Assert.Contains(1, actual);

			// コレクションに値を追加する
			mock.Object.SomeValue.Add(2);
			Assert.Equal(2, actual.Count);
			Assert.Contains(1, actual);
			Assert.Contains(2, actual);
		}

		[Fact]
		public void SetupProperty_セットしてゲットできるプロパティをエミュレートする() {
			var mock = new Mock<ITestTarget<int>>();

			// 変更できるプロパティ
			mock.SetupProperty(target => target.SomeValue, 10);

			// 初期値を確認する
			Assert.Equal(10, mock.Object.SomeValue);

			// プロパティの値を変更する
			mock.Object.SomeValue = 20;
			Assert.Equal(20, mock.Object.SomeValue);
		}

		[Fact]
		public void Setup_メソッドをエミュレートする() {
			var mock = new Mock<ITestTarget<int>>();

			mock.Setup(target => target.GetSomeValue(It.IsAny<int>()))
				.Returns((int value) => value + value);

			var result = mock.Object.GetSomeValue(1);
			Assert.Equal(2, result);
		}
	}
}
