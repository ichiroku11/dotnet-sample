using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;
public class DefaultController : Controller {
	public IActionResult Index() {
		return Content("Default");
	}
}
