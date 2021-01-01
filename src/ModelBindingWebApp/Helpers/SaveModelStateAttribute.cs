using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Helpers {
	// ModelStateを保存する属性
	public class SaveModelStateAttribute : ActionFilterAttribute {
		// リダイレクトかどうか
		private static bool IsRedirectResult(IActionResult result)
			=> result is RedirectToActionResult || result is RedirectToRouteResult;

		public override void OnActionExecuted(ActionExecutedContext context) {
			// リダイレクトのみが対象
			if (!IsRedirectResult(context.Result)) {
				return;
			}

			if (!(context.Controller is Controller controller)) {
				// ありえるの？
				return;
			}

			// バリデーションに失敗した場合が対象
			if (controller.ModelState.IsValid) {
				return;
			}

			// TempDataにModelStateを保存する
			controller.TempData.AddModelState(controller.ModelState);
		}
	}
}
