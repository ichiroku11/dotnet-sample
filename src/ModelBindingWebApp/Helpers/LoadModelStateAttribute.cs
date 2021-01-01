using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Helpers {
	// ModelStateを読み込む属性
	public class LoadModelStateAttribute : ActionFilterAttribute {
		public override void OnActionExecuting(ActionExecutingContext context) {
			if (!(context.Controller is Controller controller)) {
				// ありえるの？
				return;
			}

			// TempDataからModelStateを取り出す
			var modelState = controller.TempData.GetModelState();
			if (modelState != null) {
				controller.ModelState.Merge(modelState);
			}
		}
	}
}
