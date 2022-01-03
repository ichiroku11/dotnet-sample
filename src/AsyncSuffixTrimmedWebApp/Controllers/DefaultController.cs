using Microsoft.AspNetCore.Mvc;

namespace AsyncSuffixTrimmedWebApp.Controllers;

public class DefaultController : Controller {
	public IActionResult Index() {
		return View();
	}

	public async Task<IActionResult> SampleAsync() {
		await Task.Delay(0);
		return Content(nameof(SampleAsync));
	}

	public async Task<IActionResult> SamplePartialAsync() {
		await Task.Delay(0);
		return Content(nameof(SamplePartialAsync));
	}
}
