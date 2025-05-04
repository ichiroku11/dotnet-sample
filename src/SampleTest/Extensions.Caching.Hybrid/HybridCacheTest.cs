using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

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
	public async Task GetOrCreateAsync_factoryで指定した関数が1回だけ呼ばれることを確認する() {
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

	// スタンピード保護を確認したいが、このコードで確認できているかは不明
	[Fact]
	public async Task GetOrCreateAsync_ほぼ同時に何度もfactoryで指定した関数を呼び出しても1回だけ呼ばれることを確認する() {
		// Arrange
		var cache = GetHybridCache();

		var count = 0;
		var tasks = new ConcurrentBag<ValueTask<object>>();

		// Act
		await Parallel.ForEachAsync(Enumerable.Range(0, 10), (_, token) => {
			var task = cache.GetOrCreateAsync(
				key: "key",
				factory: token => {
					count++;
					return ValueTask.FromResult(new object());
				},
				cancellationToken: token);

			tasks.Add(task);

			return ValueTask.CompletedTask;
		});

		await Task.WhenAll(tasks.Select(task => task.AsTask()));

		// Assert
		Assert.Equal(1, count);
	}

	[Fact]
	public async Task SetAsync_キャッシュを登録する() {
		// Arrange
		var cache = GetHybridCache();

		// Act
		// キャッシュを登録する
		await cache.SetAsync(key: "key", value: "value");

		var value = await cache.GetOrCreateAsync(
			key: "key",
			// すでにキャッシュが存在するためfactoryは呼ばれない
			factory: token => {
				Assert.Fail();
				return ValueTask.FromResult("");
			});

		// Assert
		Assert.Equal("value", value);
	}

	[Fact]
	public async Task RemoveAsync_キーが存在しなくてもエラーにならないことを確認する() {
		// Arrange
		var cache = GetHybridCache();

		// Act
		await cache.RemoveAsync("key");

		// Assert
		Assert.True(true);
	}

	[Fact]
	public async Task RemoveByTagAsync_タグが存在しなくてもエラーにならないことを確認する() {
		// Arrange
		var cache = GetHybridCache();

		// Act
		await cache.RemoveByTagAsync("tag");

		// Assert
		Assert.True(true);
	}

	[Fact]
	public async Task RemoveByTagAsync_タグ文字列を使って削除する() {
		// Arrange
		var cache = GetHybridCache();

		// キャッシュを作る
		await cache.GetOrCreateAsync("key1", token => ValueTask.FromResult(""), tags: ["tag"]);
		await cache.GetOrCreateAsync("key2", token => ValueTask.FromResult(""), tags: ["tag"]);

		// Act
		// タグ文字列を使って削除する
		await cache.RemoveByTagAsync("tag");

		// 新たにキャッシュエントリが作られる
		var createdKey1 = false;
		await cache.GetOrCreateAsync("key1", token => {
			createdKey1 = true;
			return ValueTask.FromResult("");
		});

		var createdKey2 = false;
		await cache.GetOrCreateAsync("key2", token => {
			createdKey2 = true;
			return ValueTask.FromResult("");
		});

		// Assert
		Assert.True(createdKey1);
		Assert.True(createdKey2);
	}
}
