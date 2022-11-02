using Moq;

namespace SampleTest.Moq;

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
}
