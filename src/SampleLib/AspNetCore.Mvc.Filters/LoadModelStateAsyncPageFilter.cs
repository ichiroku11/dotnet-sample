using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleLib.AspNetCore.Mvc.Filters;

public class LoadModelStateAsyncPageFilter : IAsyncPageFilter {
	public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next) {
		do {
			// GETメソッドが対象
			if (!HttpMethods.IsGet(context.HttpContext.Request.Method)) {
				break;
			}

			// PageModelが対象
			if (context.HandlerInstance is not PageModel) {
				break;
			}

			// todo: TempDataからModelStateを取り出す

		} while (false);

		// ハンドラーを呼び出す
		await next();
	}

	public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) {
		return Task.CompletedTask;
	}
}
