using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace SampleLib.AspNetCore.Mvc.Filters;

/// <summary>
/// POST処理でバリデーションエラーが発生しリダイレクトしたときに
/// <see cref="ModelStateDictionary"/>を<see cref="ITempDataDictionary"/>に保存する
/// </summary>
/// <remarks>
/// MVC、Razor Pagesどちらでも利用可
/// </remarks>
public class SaveModelStateAsyncResultFilter : IAsyncResultFilter {
	private static bool IsRedirectResult(IActionResult result) {
		return
			// MVC
			result is RedirectToActionResult ||
			result is RedirectToRouteResult ||
			// Razor Pages
			result is RedirectToPageResult;
	}

	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
		// Resultを実行する
		await next.Invoke();

		// POSTメソッドが対象
		if (!HttpMethods.IsPost(context.HttpContext.Request.Method)) {
			return;
		}

		// リダイレクトが対象
		if (!IsRedirectResult(context.Result)) {
			return;
		}

		// バリデーションに失敗した場合が対象
		if (context.ModelState.IsValid) {
			return;
		}

		// TempDataにModelStateを保存する
		if (context.Controller is Controller controller) {
			controller.TempData.AddModelState(context.ModelState);
		} else if (context.Controller is PageModel pageModel) {
			pageModel.TempData.AddModelState(context.ModelState);
		}
	}
}
