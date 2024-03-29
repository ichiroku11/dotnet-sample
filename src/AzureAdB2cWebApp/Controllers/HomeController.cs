using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2cWebApp.Controllers;

[Authorize]
public class HomeController : Controller {
	[AllowAnonymous]
	public IActionResult Index() => View();

	[AllowAnonymous]
	public IActionResult Option() => View();

	public async Task<IActionResult> ClaimAsync() {
		var result = await HttpContext.AuthenticateAsync();
		ViewBag.Properties = result.Properties;

		return View();
	}
}
