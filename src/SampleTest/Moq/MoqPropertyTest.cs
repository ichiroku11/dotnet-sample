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

	[Fact]
	public void 固定値を返すプロパティに値をセットしても変更されない() {
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
}
