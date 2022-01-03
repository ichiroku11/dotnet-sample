using Microsoft.AspNetCore.Mvc;

namespace PolicyAuthzWebApp.Areas.Admin.Controllers;

[Area("Admin")]
public class DefaultController : Controller {
	public IActionResult Index() {
		return Content($"~/admin/default/{nameof(Index).ToLower()}");
	}
}
