using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleLib.AspNetCore.Mvc.Filters;

public class SaveModelStateAsyncResultFilter : IAsyncResultFilter {

	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
		await next.Invoke();

		// POSTメソッドが対象
		if (!HttpMethods.IsPost(context.HttpContext.Request.Method)) {
			return;
		}

		// リダイレクトが対象
		if (context.Result is not RedirectToPageResult) {
			return;
		}

		// PageModelが対象
		if (context.Controller is not PageModel pageModel) {
			return;
		}

		// バリデーションに失敗した場合が対象
		if (pageModel.ModelState.IsValid) {
			return;
		}

		// todo: TempDataにModelStateを保存する
	}
}
