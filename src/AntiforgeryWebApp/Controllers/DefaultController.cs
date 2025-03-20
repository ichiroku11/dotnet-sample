using Microsoft.AspNetCore.Mvc;

namespace AntiForgeryWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() => View();

	// トークンを検証する
	[HttpPost]
	public IActionResult ValidateToken() => Content(nameof(ValidateToken));

	// トークンを検証しない
	[HttpPost]
	[IgnoreAntiforgeryToken]
	public IActionResult IgnoreToken() => Content(nameof(IgnoreToken));
}
