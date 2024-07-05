using Microsoft.AspNetCore.Http.HttpResults;

namespace AzureAdB2cApiConnectorWebApp;

public class ContinuationResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "Continue";
}

public class BlockingResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "ShowBlockPage";
	public string UserMessage { get; set; } = "";
}

public class ValidationErrorResponse {
	public string Version { get; } = "1.0.0";
	public string Action { get; } = "ValidationError";
	public int Status { get; } = 400;
	public string UserMessage { get; set; } = "";
}

public static class UserCreatingEndpoints {
	public static WebApplication MapUserCreatingEndpoints(this WebApplication app) {
		var group = app.MapGroup("/user/creating");

		group.MapPost("/continue",
			async Task<Ok<ContinuationResponse>> (HttpContext context) => {
				await Task.CompletedTask;

				return TypedResults.Ok(new ContinuationResponse());
			});

		group.MapPost("/blocking",
			async Task<Ok<BlockingResponse>> (HttpContext context) => {
				await Task.CompletedTask;

				return TypedResults.Ok(new BlockingResponse { UserMessage = "Blocking!" });
			});

		group.MapPost("/validationerror",
			async Task<BadRequest<ValidationErrorResponse>> (HttpContext context) => {
				await Task.CompletedTask;

				return TypedResults.BadRequest(new ValidationErrorResponse { UserMessage = "Validation error!" });
			});

		return app;
	}
}
