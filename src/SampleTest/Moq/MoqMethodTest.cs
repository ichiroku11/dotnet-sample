using Moq;

namespace SampleTest.Moq;

public class MoqMethodTest {
	// テスト対象
	public interface ITestTarget {
		Task<int> GetValueAsync();
	}

	[Fact]
	public async void 固定値を返す非同期メソッドをテストする() {
		// Arrange
		var mock = new Mock<ITestTarget>();

		// 非同期メソッドは固定値を返す
		mock.Setup(target => target.GetValueAsync())
			.ReturnsAsync(1);

		var target = mock.Object;

		// Act
		var actual = await target.GetValueAsync();

		// Assert
		Assert.Equal(1, actual);
	}
}
