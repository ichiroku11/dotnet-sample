using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AntiforgeryWebApp.Controllers;

public class TokenSetController(IAntiforgery antiforgery, ILogger<TokenSetController> logger) : Controller {
	private readonly IAntiforgery _antiforgery = antiforgery;
	private readonly ILogger<TokenSetController> _logger = logger;

	public IActionResult GetTokens() {
		var tokenSet = _antiforgery.GetTokens(HttpContext);

		_logger.LogInformation("{method} cookie: {cookie}", nameof(GetTokens), Request.Headers.Cookie);
		_logger.LogInformation("{method} set-cookie: {setCookie}", nameof(GetTokens), Response.Headers.SetCookie);

		return Json(tokenSet);
	}

	public IActionResult GetAndStoreTokens() {
		var tokenSet = _antiforgery.GetAndStoreTokens(HttpContext);

		_logger.LogInformation("{method} cookie: {cookie}", nameof(GetAndStoreTokens), Request.Headers.Cookie);
		_logger.LogInformation("{method} set-cookie: {setCookie}", nameof(GetAndStoreTokens), Response.Headers.SetCookie);

		return Json(tokenSet);
	}
}
