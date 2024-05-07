using Moq;

namespace SampleTest.Moq;

// 参考
// https://github.com/Moq/moq4/wiki/Quickstart#methods
public class MockMethodTest {
	// テスト対象
	public interface ITestTarget {
		int GetValue();
		int GetValue(int value);

		Task<int> GetValueAsync();
	}

	[Fact]
	public void 固定値を返すメソッドをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		// メソッドは固定値を返す
		mock.Setup(target => target.GetValue()).Returns(1);

		var target = mock.Object;

		// Act
		var actual = target.GetValue();

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void 引数があり固定値を返すメソッドをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		// メソッドはint型の引数を受け取り固定値を返す
		mock.Setup(target => target.GetValue(It.IsAny<int>())).Returns(1);

		var target = mock.Object;

		// Act
		var actual = target.GetValue(0);

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void 引数の値によって戻り値が変わるメソッドをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		mock.Setup(target => target.GetValue(It.Is<int>(value => value % 2 == 0))).Returns(0);
		mock.Setup(target => target.GetValue(It.Is<int>(value => value % 2 == 1))).Returns(1);

		var target = mock.Object;

		// Act
		// Assert
		Assert.Equal(0, target.GetValue(0));
		Assert.Equal(1, target.GetValue(1));
	}

	[Fact]
	public async Task 固定値を返す非同期メソッドをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		mock.Setup(target => target.GetValueAsync()).ReturnsAsync(1);

		var target = mock.Object;

		// Act
		var actual = await target.GetValueAsync();

		// Assert
		Assert.Equal(1, actual);
	}
}
