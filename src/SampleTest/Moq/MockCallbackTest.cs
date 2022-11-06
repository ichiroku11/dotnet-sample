using Moq;

namespace SampleTest.Moq;

public class MockCallbackTest {
	public interface ITestTarget {
		bool Add(int value);
	}

	[Fact]
	public void Callback_メソッドが呼び出されたときに処理する() {
		// Arrange
		var values = new List<int>();
		var mock = new Mock<ITestTarget>();
		mock.Setup(target => target.Add(It.IsAny<int>()))
			// 要素をリストに追加する
			.Callback<int>(value => values.Add(value))
			.Returns(true);

		var target = mock.Object;

		// Act
		// Assert
		Assert.True(target.Add(1));
		Assert.Single(values);
		Assert.Equal(1, values.First());

		Assert.True(target.Add(2));
		Assert.Collection(values,
			value => Assert.Equal(1, value),
			value => Assert.Equal(2, value));
	}
}
