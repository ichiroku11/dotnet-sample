using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DistributedCacheWebApp.Controllers {
	public class CacheController : Controller {
		private readonly IDistributedCache _cache;
		private const string _cacheKey = "cache-sample";

		public CacheController(IDistributedCache cache) {
			_cache = cache;
		}

		public async Task<IActionResult> Get() {
			// キャッシュから取得
			var value = await _cache.GetStringAsync(_cacheKey);
			if (string.IsNullOrWhiteSpace(value)) {
				return Content("get error");
			}

			return Content($"get: {value}");
		}

		public async Task<IActionResult> Set(string value) {
			if (string.IsNullOrEmpty(value)) {
				return Content("set error");
			}

			// キャッシュに設定
			await _cache.SetStringAsync(_cacheKey, $"{value}");

			return Content($"set: {value}");
		}

		public async Task<IActionResult> Remove() {
			// キャッシュから削除
			await _cache.RemoveAsync(_cacheKey);

			return Content($"remove");
		}
	}
}
