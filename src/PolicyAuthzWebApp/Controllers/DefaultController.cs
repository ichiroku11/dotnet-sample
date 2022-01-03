using Microsoft.AspNetCore.Mvc;

namespace PolicyAuthzWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() {
		return Content($"~/default/{nameof(Index).ToLower()}");
	}
}
