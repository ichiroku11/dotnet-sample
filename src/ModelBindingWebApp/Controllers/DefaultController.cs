using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;
using SampleLib.AspNetCore.Mvc.Filters;

namespace ModelBindingWebApp.Controllers;

// ModelStateをTempDataに保存し、TempDataから読み出すサンプル
public class DefaultController : Controller {
	[TypeFilter(typeof(LoadModelStateAsyncActionFilter))]
	public IActionResult Index() {
		return View(new UserUpdateCommand { });
	}

	[HttpPost]
	[TypeFilter(typeof(SaveModelStateAsyncResultFilter))]
	public IActionResult Index(UserUpdateCommand command) {
		if (!ModelState.IsValid) {
			return RedirectToAction();
		}

		// 仮
		return Content("保存しました！");
	}
}
