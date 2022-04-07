using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers;

[Route("api/[controller]")]
public class Base64JsonController : Controller {
	// ~/api/base64json
	[HttpPost]
	//[Consumes("text/plain")]
	public async Task<Sample> PostAsync(
		// FromBody属性を設定するとtext/plainをバインドできない様子
		//[FromBody]
		[ModelBinder(typeof(SampleBase64JsonModelBinder))]
		Sample sample) {
		await Task.CompletedTask;
		return sample;
	}
}
