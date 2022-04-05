using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers;

[Route("api/[controller]")]
public class Base64JsonController : Controller {
	// ~/api/base64json
	[HttpPost]
	public async Task<Sample> PostAsync(Sample sample) {
		await Task.CompletedTask;
		return sample;
	}
}
