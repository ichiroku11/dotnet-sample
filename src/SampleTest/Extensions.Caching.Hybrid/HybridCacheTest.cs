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

	private static HybridCache GetHybridCache() {
		var services = new ServiceCollection();
		services.AddHybridCache();
		var provider = services.BuildServiceProvider();
		return provider.GetRequiredService<HybridCache>();
	}

	[Fact]
	public async Task GetOrCreateAsync_基本的な使い方を確認する() {
		// Arrange
		var cache = GetHybridCache();

		// Act
		var value = await cache.GetOrCreateAsync(
			key: "key",
			factory: token => ValueTask.FromResult("value"));

		// Assert
		Assert.Equal("value", value);
	}

	[Fact]
	public async Task GetOrCreateAsync_factoryで指定した関数が最初だけ呼ばれることを確認する() {
		// Arrange
		var cache = GetHybridCache();

		var count = 0;
		ValueTask<string> factory(CancellationToken token) {
			count++;
			return ValueTask.FromResult("value");
		}

		// Act
		var value1 = await cache.GetOrCreateAsync(key: "key", factory: factory);
		var value2 = await cache.GetOrCreateAsync(key: "key", factory: factory);

		// Assert
		Assert.Equal(1, count);
		Assert.Equal("value", value1);
		Assert.Equal("value", value2);
	}
}
