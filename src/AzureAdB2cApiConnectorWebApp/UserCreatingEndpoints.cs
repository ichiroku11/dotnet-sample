using Microsoft.AspNetCore.Http.HttpResults;

namespace AzureAdB2cApiConnectorWebApp;

public static class UserCreatingEndpoints {
	// 参考
	// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-api-connector?pivots=b2c-user-flow
	public static WebApplication MapUserCreatingEndpoints(this WebApplication app) {
		var group = app.MapGroup("/user/creating");

		// ハンドラーでは次のことをするはず
		// - Basic認証の検証
		// - context.Request.BodyからJSONを読み込み

		// 継続応答を返す
		group.MapPost(
			"/continue",
			async Task<Ok<ContinuationResponse>> (HttpContext _) => {
				await Task.CompletedTask;

				return TypedResults.Ok(new ContinuationResponse());
			});

		// ブロック応答を返す
		group.MapPost(
			"/blocking",
			async Task<Ok<BlockingResponse>> (HttpContext _) => {
				await Task.CompletedTask;

				return TypedResults.Ok(new BlockingResponse { UserMessage = "Blocking!" });
			});

		// 検証エラー応答を返す
		group.MapPost(
			"/validationerror",
			async Task<BadRequest<ValidationErrorResponse>> (HttpContext _) => {
				await Task.CompletedTask;

				// ステータスコードも400を返す必要がある
				return TypedResults.BadRequest(new ValidationErrorResponse { UserMessage = "Validation error!" });
			});

		return app;
	}
}

// 継続応答
// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-api-connector?pivots=b2c-user-flow#example-of-a-continuation-response
public class ContinuationResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "Continue";
}

// ブロック応答
// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-api-connector?pivots=b2c-user-flow#example-of-a-blocking-response
public class BlockingResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "ShowBlockPage";
	public string UserMessage { get; set; } = "";
}

// 検証エラー応答
// https://learn.microsoft.com/ja-jp/azure/active-directory-b2c/add-api-connector?pivots=b2c-user-flow#example-of-a-validation-error-response
public class ValidationErrorResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "ValidationError";
	public int Status { get; } = 400;
	public string UserMessage { get; set; } = "";
}
