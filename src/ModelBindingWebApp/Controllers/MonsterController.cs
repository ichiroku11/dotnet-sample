using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;
using SampleLib.AspNetCore.Mvc.Filters;

namespace ModelBindingWebApp.Controllers;

// あるあるなモデルバインドのサンプル
public class MonsterController : Controller {
	[TypeFilter(typeof(LoadModelStateAsyncActionFilter))]
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
	[TypeFilter(typeof(SaveModelStateAsyncResultFilter))]
	public IActionResult Save(
		// POSTする部分はViewModelのプロパティに指定しているため
		// Bind属性でPrefixを指定する
		[Bind(Prefix = nameof(MonsterViewModel.Form))] MonsterFormModel formModel) {
		if (!ModelState.IsValid) {
			return RedirectToAction(nameof(Index));
		}

		return RedirectToAction(nameof(Index));
	}
}
