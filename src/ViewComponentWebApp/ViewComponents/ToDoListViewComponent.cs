using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewComponentWebApp.Models;

namespace ViewComponentWebApp.ViewComponents;

// 参考
// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/views/view-components?view=aspnetcore-5.0
// ViewComponentから派生する
public class TodoListViewComponent : ViewComponent {
	private readonly TodoRepository _repository;

	public TodoListViewComponent(TodoRepository repository) {
		_repository = repository;
	}

	// InvokeAsyncメソッドを用意する
	public async Task<IViewComponentResult> InvokeAsync(bool isDone) {
		ViewBag.Todos = await _repository.GetTodosAsync(isDone);

		return View();
	}
}
