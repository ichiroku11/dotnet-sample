using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SampleLib.AspNetCore.Mvc.Filters;

/// <summary>
/// <see cref="ITempDataDictionary"/>に保存された<see cref="ModelStateDictionary"/>を読み出す
/// </summary>
/// <remarks>
/// MVCでのみ利用可
/// </remarks>
public class LoadModelStateAsyncActionFilter : IAsyncActionFilter {
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
		// GETメソッドかつControllerが対象
		if (HttpMethods.IsGet(context.HttpContext.Request.Method) &&
			context.Controller is Controller controller) {
			// TempDataからModelStateを取り出す
			var modelState = controller.TempData.GetModelState();
			if (modelState is not null) {
				context.ModelState.Merge(modelState);
			}
		}

		// ハンドラーを呼び出す
		await next();
	}
}
