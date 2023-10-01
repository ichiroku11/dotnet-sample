using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Controllers;

// ModelBindingMessageProviderをテストする
[Route("api/messageprovider/[action]")]
[ApiController]
public class MessageProviderController : ControllerBase {
	// MissingBindRequiredValueAccessorのテスト
	public class MissingBindRequiredModel {
		[BindRequired]
		public int Value { get; set; }
	}

	[HttpPost]
	public IActionResult MissingBindRequired([FromForm] MissingBindRequiredModel model) => Ok(model);


	// ValueMustNotBeNullAccessorのテスト
	public class ValueMustNotBeNullModel {
		[Required]
		public string Value { get; set; } = "";
	}

	[HttpPost]
	public IActionResult ValueMustNotBeNull([FromForm] ValueMustNotBeNullModel model) => Ok(model);
}
