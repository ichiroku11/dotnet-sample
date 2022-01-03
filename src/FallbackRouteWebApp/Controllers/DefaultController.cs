using Microsoft.AspNetCore.Mvc;

namespace FallbackRouteWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() => new EmptyResult();
}
