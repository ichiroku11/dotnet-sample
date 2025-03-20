using Microsoft.AspNetCore.Mvc;

namespace AntiForgeryWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() => View();
}
