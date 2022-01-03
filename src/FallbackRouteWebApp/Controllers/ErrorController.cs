using Microsoft.AspNetCore.Mvc;

namespace FallbackRouteWebApp.Controllers;

public class ErrorController : Controller {
	// 親クラスでNotFoundは定義されているのでnew
	public new IActionResult NotFound() => Content("Not Found");
}
