using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionTest {
	// 当然だろうが念のため
	[Fact]
	public void インスタンスを生成した直後の要素は空() {
		// Arrange
		// Act
		var services = new ServiceCollection();

		// Assert
		Assert.Empty(services);
	}
}
