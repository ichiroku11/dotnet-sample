using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/collectioncount")]
[ApiController]
public class ValidationCollectionCountController : ControllerBase {
	public class Model {
		[CollectionCount(3, Min = 2)]
		public IEnumerable<int> Values { get; init; } = new List<int>();
	}

	[HttpPost]
	public IActionResult Test(Model model) {
		return Ok(model);
	}
}
