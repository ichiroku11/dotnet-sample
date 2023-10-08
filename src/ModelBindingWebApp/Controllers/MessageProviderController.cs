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
		public int Value { get; init; }
	}

	[HttpPost]
	public IActionResult MissingBindRequired([FromForm] MissingBindRequiredModel model) => Ok(model);


	// ValueMustNotBeNullAccessorのテスト
	public class ValueMustNotBeNullModel {
		public int Value { get; init; }
	}

	[HttpPost]
	public IActionResult ValueMustNotBeNull([FromForm] ValueMustNotBeNullModel model) => Ok(model);


	// MissingKeyOrValueAccessorのテスト
	public class MissingKeyOrValueModel {
		public Dictionary<string, string> Values { get; init; } = new();
	}

	[HttpPost]
	public IActionResult MissingKeyOrValue([FromForm] MissingKeyOrValueModel model) => Ok(model);

}
