using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Controllers;

[Route("api/validation/[action]")]
[ApiController]
public class ValidationNestedController : ControllerBase {
	// 属性バリデーションの確認
	public class InnerModel {
		[Required(ErrorMessage = $"{nameof(InnerModel)}.{nameof(Value)} is required.")]
		public string Value { get; init; } = "";
	}

	public class OuterModel {
		[Required(ErrorMessage = $"{nameof(OuterModel)}.{nameof(Inner)} is required.")]
		public InnerModel? Inner { get; init; } = null;
	}

	[HttpPost]
	public IActionResult Nested(OuterModel model) => Ok(model);

	// IValidatableObjectによるバリデーションを確認
	public class ValidatableInnerModel : IValidatableObject {
		public string Value { get; init; } = "";

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			if (string.IsNullOrEmpty(Value)) {
				yield return new ValidationResult($"{nameof(ValidatableInnerModel)}.{nameof(Value)} is required.", [nameof(Value)]);
			}
		}
	}

	public class ValidatableOuterModel : IValidatableObject {
		public ValidatableInnerModel? Inner { get; init; } = null;

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			if (Inner is null) {
				yield return new ValidationResult($"{nameof(ValidatableOuterModel)}.{nameof(Inner)} is required.", [nameof(Inner)]);
			}
		}
	}

	[HttpPost]
	public IActionResult ValidatableNested(ValidatableOuterModel model) => Ok(model);
}
