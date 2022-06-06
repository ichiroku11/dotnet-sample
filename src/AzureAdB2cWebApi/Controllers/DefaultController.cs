using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2cWebApi.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() => Content("Hello World!");
}
