using Microsoft.AspNetCore.Mvc;

namespace ControllerWebApp.Controllers;

public class AccountController : AppController {
	public IActionResult Index() => Content($"Account.{nameof(Index)}");
}
