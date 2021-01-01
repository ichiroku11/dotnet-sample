using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBindingWebApp.Models;

namespace ModelBindingWebApp.Controllers {
	// ディクショナリへのバインドを試す
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/models/model-binding?view=aspnetcore-3.1

	[Route("api/[controller]")]
	[ApiController]
	public class DictionaryController : ControllerBase {
		// 何か時間がかかる処理
		private Task ActionAsync() => Task.CompletedTask;

		// ~/api/dictionary
		[HttpPost]
		public async Task<IDictionary<string, int>> PostAsync(
			// Formデータをバインドする
			[FromForm] IDictionary<string, int> values) {
			await ActionAsync();

			return values;
		}

		// どうもバインドできない様子
		// NotSupportedExceptionがスローされる
		// The collection type 'System.Collections.Generic.Dictionary`2[System.Int32,MiscWebApi.Models.Sample]' is not supported.
		// ~/api/dictionary/complex
		[HttpPost("complex")]
		public async Task<IDictionary<int, Sample>> PostAsync(
			[FromForm] IDictionary<int, Sample> values) {
			await ActionAsync();

			return values;
		}
	}
}
