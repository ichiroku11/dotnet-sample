using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/collectionlength/[action]")]
[ApiController]
public class ValidationCollectionLengthController : ControllerBase {
	public class Model {
		[CollectionLength(10, MinLength = 1)]
		public IEnumerable<string> Values { get; init; } = new List<string>();
	}

	public IActionResult Test(Model model) {
		return Ok(model);
	}
}
