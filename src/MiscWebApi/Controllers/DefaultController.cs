using Microsoft.AspNetCore.Mvc;

namespace MiscWebApi.Controllers;

// ApiExplorerでは取得できない
public class DefaultController : Controller {
	public IActionResult Index() => View();
	public IActionResult Test() => Json(new { x = 1 });
}
