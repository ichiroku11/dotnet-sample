using Moq;

namespace SampleTest.Moq;

// 参考
// https://github.com/Moq/moq4/wiki/Quickstart#linq-to-mocks
public class MockLinqTest {
	// テスト対象
	public interface ITestTarget {
		int GetValue();
	}

	[Fact]
	public void Of_宣言的にモックを作成する() {
		// Arrange
		var target = Mock.Of<ITestTarget>(target => target.GetValue() == 1);

		// Act
		var actual = target.GetValue();

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void Get_Mockを取得する() {
		// Arrange
		var target = Mock.Of<ITestTarget>(target => target.GetValue() == 1);

		// モックオブジェクトを取得して再設定（上書き）してみる
		var mock = Mock.Get(target);
		mock.Setup(target => target.GetValue()).Returns(2);

		// Act
		var actual = target.GetValue();

		// Assert
		Assert.Equal(2, actual);
	}
}
