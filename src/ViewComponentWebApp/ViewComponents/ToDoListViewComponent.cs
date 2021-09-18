using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewComponentWebApp.ViewComponents {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/views/view-components?view=aspnetcore-5.0
	// ViewComponentから派生する
	public class TodoListViewComponent : ViewComponent {

		// InvokeAsyncメソッドを用意する
		public async Task<IViewComponentResult> InvokeAsync() {

			await Task.CompletedTask;

			return View();
		}
	}
}
