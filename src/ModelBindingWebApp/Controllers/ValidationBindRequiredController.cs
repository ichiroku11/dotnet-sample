using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBindingWebApp.Controllers;

// BindRequired属性によるバリデーションを確認する
[Route("api/validation/bindrequired/[action]")]
[ApiController]
public class ValidationBindRequiredController : ControllerBase {
	// 値型のプロパティを持つモデル
	public record ValueTypeModel([BindRequired] int Value);

	[HttpPost]
	public IActionResult ValueType(ValueTypeModel model) => Ok(model);


	// null許容値型のプロパティを持つモデル
	public record NullableValueTypeModel([BindRequired] int? Value);

	[HttpPost]
	public IActionResult NullableValueType(NullableValueTypeModel model) => Ok(model);


	// 参照型のプロパティを持つモデル
	public record ReferenceTypeModel([BindRequired] string Value = "");

	[HttpPost]
	public IActionResult ReferenceType(ReferenceTypeModel model) => Ok(model);


	// null許容参照型のプロパティを持つモデル
	public record NullableReferenceTypeModel([BindRequired] string? Value);

	[HttpPost]
	public IActionResult NullableReferenceType(NullableReferenceTypeModel model) => Ok(model);
}
