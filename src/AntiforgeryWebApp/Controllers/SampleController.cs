using Microsoft.AspNetCore.Mvc;

namespace AntiforgeryWebApp.Controllers;

public class SampleController : Controller {
	public IActionResult Index() => View();

	// トークンを検証する
	[HttpPost]
	public IActionResult ValidateToken() => Content(nameof(ValidateToken));

	// トークンを検証しない
	[HttpPost]
	[IgnoreAntiforgeryToken]
	public IActionResult IgnoreToken() => Content(nameof(IgnoreToken));
}
