using Microsoft.AspNetCore.Mvc;

namespace MiddlewareWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() {
		return Content("Hello Middleware!");
	}
}
