using Moq;

namespace SampleTest.Moq;

public class MoqMethodTest {
	// テスト対象
	public interface ITestTarget {
		int GetValue();

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
	public async void 固定値を返す非同期メソッドをテストする() {
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
