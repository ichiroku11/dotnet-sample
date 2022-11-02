using Moq;

namespace SampleTest.Moq;

// 参考
// https://github.com/Moq/moq4/wiki/Quickstart#properties
public class MoqPropertyTest {
	// テスト対象
	public interface ITestTarget {
		int Value { get; set; }
		IList<int> Values { get; set; }
	}

	[Fact]
	public void Setupしていないプロパティは初期値を返す様子() {
		// Arrange
		var mock = new Mock<ITestTarget>();
		var target = mock.Object;

		// Act
		// Assert
		Assert.Equal(0, target.Value);
		Assert.Null(target.Values);
	}

	[Fact]
	public void 固定値を返すプロパティをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		// プロパティは固定値を返す
		mock.Setup(target => target.Value).Returns(1);

		var target = mock.Object;

		// Act
		var actual = target.Value;

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void 固定値を返すプロパティに値を設定しても変更されない() {
		// Arrange
		var mock = new Mock<ITestTarget>();
		mock.Setup(target => target.Value).Returns(1);

		var target = mock.Object;

		// Act
		target.Value = 2;

		// Assert
		// プロパティの値を変更しても、取得できるのはセットアップで設定した値
		Assert.Equal(1, target.Value);
	}

	[Fact]
	public void コレクションのプロパティをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		var expected = new List<int> { 1 };
		mock.Setup(target => target.Values).Returns(expected);

		var target = mock.Object;

		// Act
		// Assert
		// Returnsで指定したインスタンスを取得できる
		Assert.Same(expected, target.Values);

		// Returnsで指定したインスタンスに含まれている値を取得できる
		Assert.Single(target.Values, 1);

		// コレクションに値を追加できる
		target.Values.Add(2);
		Assert.Collection(target.Values,
			value => Assert.Equal(1, value),
			value => Assert.Equal(2, value));
	}

	[Fact]
	public void SetupPropertyメソッドを使って設定して取得できるプロパティをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		// 設定・取得できるプロパティ
		mock.SetupProperty(target => target.Value, 1);

		var target = mock.Object;

		// Act
		// Assert
		// 初期値を取得できる
		Assert.Equal(1, target.Value);

		// 変更した値を取得できる
		target.Value = 2;
		Assert.Equal(2, target.Value);
	}
}
