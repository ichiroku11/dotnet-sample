using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Helpers;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers {
	// あるあるなモデルバインドのサンプル
	public class MonsterController : Controller {
		[LoadModelState]
		public IActionResult Index() {
			var viewModel = new MonsterViewModel {
				CategorySelectListItems = Enum.GetValues(typeof(MonsterCategory))
					.OfType<MonsterCategory>()
					.Select(category => category.ToSelectListItem()),
				Form = new MonsterFormModel {
					Id = 1,
					Category = MonsterCategory.Slime,
					Name = "スライム"
				},
			};

			return View(viewModel);
		}

		[HttpPost]
		[SaveModelState]
		public IActionResult Save(
			// POSTする部分はViewModelのプロパティに指定しているため
			// Bind属性でPrefixを指定する
			[Bind(Prefix = nameof(MonsterViewModel.Form))]MonsterFormModel formModel) {
			if (!ModelState.IsValid) {
				return RedirectToAction(nameof(Index));
			}

			return RedirectToAction(nameof(Index));
		}
	}
}
