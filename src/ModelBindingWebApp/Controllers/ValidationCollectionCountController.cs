using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/collectioncount")]
[ApiController]
public class ValidationCollectionCountController : ControllerBase {
	[HttpPost]
	public IActionResult Test([CollectionCount(3, Min = 2)] IEnumerable<int> values) {
		return Ok(values);
	}
}
