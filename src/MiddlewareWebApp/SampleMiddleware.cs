using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiddlewareWebApp {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1

	// ミドルウェア
	// - RequestDelegate型のパラメータを持つパブリックコンストラクタ
	// - HttpContext型のパラメータを持つInvokeメソッドかInvokeAsyncメソッド
	// を持つ

	// サンプルミドルウェア
	public class SampleMiddleware {
		private static readonly JsonSerializerOptions _options
			= new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

		private readonly RequestDelegate _next;
		private readonly ILogger _logger;
		private readonly string _label;

		public SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger, string label) {
			_next = next;
			_logger = logger;
			_label = label;
		}

		private void Log(HttpContext context, bool beforeRequestDelegate) {
			var routeValues = context.Request.RouteValues;

			var json = new {
				label = _label,
				beforeRequestDelegate,
				controller = routeValues["controller"],
				action = routeValues["action"],
			};
			_logger.LogInformation(JsonSerializer.Serialize(json, _options));
		}

		public async Task InvokeAsync(HttpContext context) {
			Log(context, true);

			await _next(context);

			Log(context, false);
		}
	}
}
