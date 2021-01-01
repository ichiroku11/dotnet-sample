using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers {
	// コレクションへのバインドを試す
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1

	[Route("api/[controller]")]
	[ApiController]
	public class CollectionController : ControllerBase {
		// 何か時間がかかる処理
		private Task ActionAsync() => Task.CompletedTask;

		// ~/api/collection
		[HttpPost]
		public async Task<IEnumerable<int>> PostAsync(
			// Formデータをバインドする
			[FromForm] IEnumerable<int> values) {
			await ActionAsync();

			return values;
		}

		// ~/api/collection/complex
		[HttpPost("complex")]
		public async Task<IEnumerable<Sample>> PostAsync(
			[FromForm] IEnumerable<Sample> values) {
			await ActionAsync();

			return values;
		}
	}
}
