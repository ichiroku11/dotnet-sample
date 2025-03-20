using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace AntiforgeryWebApp.Controllers;

public class TokenSetController(IAntiforgery antiforgery) : Controller {
	private readonly IAntiforgery _antiforgery = antiforgery;

	public IActionResult GetTokens() {
		var tokenSet = _antiforgery.GetTokens(HttpContext);

		return Json(tokenSet);
	}

	public IActionResult GetAndStoreTokens() {
		var tokenSet = _antiforgery.GetAndStoreTokens(HttpContext);

		return Json(tokenSet);
	}
}
