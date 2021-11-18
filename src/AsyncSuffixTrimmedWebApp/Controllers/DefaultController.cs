using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
