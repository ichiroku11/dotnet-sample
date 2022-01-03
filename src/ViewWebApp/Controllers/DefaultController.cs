using Microsoft.AspNetCore.Mvc;

namespace ViewWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() {
		return View();
	}
}
