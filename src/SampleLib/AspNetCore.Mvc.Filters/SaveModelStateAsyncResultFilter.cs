using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SampleLib.AspNetCore.Mvc.Filters;

public class SaveModelStateAsyncResultFilter : IAsyncResultFilter {
	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) {
		await next.Invoke();

		// todo: Post

		if (context.Result is not RedirectToPageResult) {
			return;
		}

		// todo: Controller
		if (context.Controller is not PageModel pageModel) {
			return;
		}

		if (pageModel.ModelState.IsValid) {
			return;
		}

		// todo: ModelStateを保存する

	}
}
