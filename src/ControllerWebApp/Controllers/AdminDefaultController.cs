using Microsoft.AspNetCore.Mvc;

namespace ControllerWebApp.Controllers;

[Area("Admin")]
public class AdminDefaultController : AppController {
	public IActionResult Index() => Content($"AdminDefault.{nameof(Index)}");
}
