using Microsoft.AspNetCore.Mvc;

namespace FeatureFlagWebApp.Controllers;
public class HomeController : Controller {
	public IActionResult Index() {
		return Content("Hello World!");
	}
}
