using Microsoft.AspNetCore.Mvc;

namespace MiscWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class RequestBodyController : ControllerBase {
	public class Sample {
		public string Value { get; init; } = "";
	}

	// The request body can only be read once, from beginning to end.
	// 要求本文は、最初から最後まで1回のみ読み取ることができます
	// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/use-http-context?view=aspnetcore-7.0#enable-request-body-buffering

	// モデルにバインド済みなのでRequest.Bodyの中身は空文字
	[HttpPost]
	public async Task<IActionResult> JsonWithBind([FromBody] Sample model) {
		// bodyは空文字
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();

		return Ok(new { body, model.Value });
	}

	// モデルにバインドしていないのでRequest.Bodyから読み取れる
	[HttpPost]
	public async Task<IActionResult> JsonWithoutBind() {
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();

		return Ok(new { body });
	}

	// モデルにバインド済みなのでRequest.Bodyの中身は空文字
	[HttpPost]
	public async Task<IActionResult> FormWithBind([FromForm] Sample model) {
		// bodyは空文字
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();

		return Ok(new { value = model.Value, body });
	}

	// モデルにバインドしていないのでRequest.Bodyから読み取れる
	[HttpPost]
	public async Task<IActionResult> FormWithoutBind() {
		using var reader = new StreamReader(Request.Body);
		var body = await reader.ReadToEndAsync();

		return Ok(new { body });
	}
}
