using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SampleLib.AspNetCore.Mvc.Filters;

/// <summary>
/// <see cref="ITempDataDictionary"/>に保存された<see cref="ModelStateDictionary"/>を読み出す
/// </summary>
/// <remarks>
/// Razor Pagesでのみ利用可
/// </remarks>
public class LoadModelStateAsyncPageFilter : IAsyncPageFilter {
	public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next) {
		// GETメソッドかつPageModelが対象
		if (HttpMethods.IsGet(context.HttpContext.Request.Method) &&
			context.HandlerInstance is PageModel pageModel) {
			// TempDataからModelStateを取り出す
			var modelState = pageModel.TempData.GetModelState();
			if (modelState is not null) {
				context.ModelState.Merge(modelState);
			}
		}

		// ハンドラーを呼び出す
		await next();
	}

	public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) {
		return Task.CompletedTask;
	}
}
