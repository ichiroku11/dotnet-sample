using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Controllers;

// Required属性によるバリデーションを確認する
// 参考）
// https://learn.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-7.0#required-validation-on-the-server
// A non-nullable field is always valid, and the [Required] attribute's error message is never displayed.
// NULLでないフィールドは常に有効であり、[Required]属性のエラーメッセージが表示されることはない。
[Route("api/validation/required/[action]")]
[ApiController]
public class ValidationRequiredController : ControllerBase {
	// 値型のプロパティを持つモデル
	// 値が送信されなくてもバリデーションエラーにならない
	public record ValueTypeModel([Required] int Value);

	[HttpPost]
	public IActionResult ValueType(ValueTypeModel model) => Ok(model);


	// null許容値型のプロパティを持つモデル
	// 値が送信されないとバリデーションエラーになる
	public record NullableValueTypeModel([Required] int? Value);

	[HttpPost]
	public IActionResult NullableValueType(NullableValueTypeModel model) => Ok(model);


	// 参照型のプロパティを持つモデル
	// 値が送信されないとバリデーションエラーになる
	public record ReferenceTypeModel([Required] string Value = "");

	[HttpPost]
	public IActionResult ReferenceType(ReferenceTypeModel model) => Ok(model);


	// null許容参照型のプロパティを持つモデル
	// 値が送信されないとバリデーションエラーになる
	public record NullableReferenceTypeModel([Required] string? Value);

	[HttpPost]
	public IActionResult NullableReferenceType(NullableReferenceTypeModel model) => Ok(model);
}
