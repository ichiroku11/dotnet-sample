using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBindingWebApp.Controllers;

// ModelBindingMessageProviderをテストする
[Route("api/messageprovider/[action]")]
[ApiController]
public class MessageProviderController : ControllerBase {
	public class MissingBindRequiredModel {
		[BindRequired]
		public int Value { get; set; }
	}

	[HttpPost]
	public IActionResult MissingBindRequired([FromForm]MissingBindRequiredModel model) {
		return Ok(model);
	}
}
