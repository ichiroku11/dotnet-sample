using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/allstringlength")]
[ApiController]
public class ValidationAllStringLengthController : ControllerBase {
	[HttpPost]
	public IActionResult Test([AllStringLength(10, Min = 5)] IEnumerable<string> values) {
		return Ok(values);
	}
}
