using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.Caching.Hybrid;

public class HybridCacheTest {
	// HybridCacheクラスは抽象クラスなのでインスタンス化できない
	// DIで取得する必要がある
	[Fact]
	public void Constructor_インスタンスはDIで取得する必要がある() {
		// Arrange
		var services = new ServiceCollection();
		services.AddHybridCache();
		var provider = services.BuildServiceProvider();

		// Act
		var cache = provider.GetRequiredService<HybridCache>();

		// Assert
		Assert.IsAssignableFrom<HybridCache>(cache);
	}
}
