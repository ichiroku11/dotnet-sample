using System.Text.Json;

namespace MiddlewareWebApp;

// 参考
// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1

// ミドルウェア
// - RequestDelegate型のパラメータを持つパブリックコンストラクタ
// - HttpContext型のパラメータを持つInvokeメソッドかInvokeAsyncメソッド
// を持つ

// サンプルミドルウェア
public class SampleMiddleware(RequestDelegate next, ILogger<SampleMiddleware> logger, string label) {
	private static readonly JsonSerializerOptions _options
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	private readonly RequestDelegate _next = next;
	private readonly ILogger _logger = logger;
	private readonly string _label = label;

	private void Log(HttpContext context, bool beforeRequestDelegate) {
		var routeValues = context.Request.RouteValues;

		var json = JsonSerializer.Serialize(
			new {
				label = _label,
				beforeRequestDelegate,
				controller = routeValues["controller"],
				action = routeValues["action"],
			},
			_options);
		_logger.LogInformation("{json}", json);
	}

	public async Task InvokeAsync(HttpContext context) {
		Log(context, true);

		await _next(context);

		Log(context, false);
	}
}
