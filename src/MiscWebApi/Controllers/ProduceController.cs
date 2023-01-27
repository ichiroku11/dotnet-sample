using Microsoft.AspNetCore.Mvc;

namespace MiscWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ProduceController : ControllerBase {
	[HttpGet]
	public IActionResult Default() => Ok(new { });

	[HttpGet]
	[Produces("text/json")]
	public IActionResult TextJson() => Ok(new { });
}
