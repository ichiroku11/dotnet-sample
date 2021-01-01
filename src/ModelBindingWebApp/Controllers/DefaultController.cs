using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers {
	// ModelStateをTempDataに保存し、TempDataから読み出すサンプル
	public class DefaultController : Controller {
		[LoadModelState]
		public IActionResult Index() {
			// OnActionExecutingでModelStateをTempDataから復元する
			return View(new UserUpdateCommand { });
		}

		[HttpPost]
		[SaveModelState]
		public IActionResult Index(UserUpdateCommand command) {
			if (!ModelState.IsValid) {
				// OnActionExecutedでModelStateをTempDataに保存する
				return RedirectToAction();
			}

			// 仮
			return Content("保存しました！");
		}
	}
}
