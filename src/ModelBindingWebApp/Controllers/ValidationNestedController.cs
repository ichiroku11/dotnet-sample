using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/nested")]
[ApiController]
public class ValidationNestedController : ControllerBase {
	public class InnerModel {
		[Required(ErrorMessage = $"{nameof(Value)} is required.")]
		public string Value { get; init; } = "";
	}

	public class OuterModel {
		[Required(ErrorMessage = $"{nameof(Inner)} is required.")]
		public InnerModel? Inner { get; init; } = null;
	}

	[HttpPost]
	public IActionResult Nested(OuterModel model) => Ok(model);
}
