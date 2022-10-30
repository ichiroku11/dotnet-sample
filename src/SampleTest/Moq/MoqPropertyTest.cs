using Moq;

namespace SampleTest.Moq;

public class MoqPropertyTest {
	// テスト対象
	public interface ITestTarget {
		int Value { get; set; }
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
}
