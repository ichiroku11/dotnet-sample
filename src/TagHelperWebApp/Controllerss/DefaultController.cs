using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TagHelperWebApp.Models;

namespace TagHelperWebApp.Controllerss {
	public class DefaultController : Controller {
		private readonly ILogger _logger;

		public DefaultController(ILogger<DefaultController> logger) {
			_logger = logger;
		}

		public IActionResult Index() {
			return View();
		}

		public IActionResult Form() {
			return View();
		}

		[HttpPost]
		public IActionResult Form(FormCommand command) {
			// ModelStateEntry.RawValueはstringかstring[]っぽい
			// checkboxにチェックを入れるとstring[]
			foreach (var entry in ModelState) {
				var builder = new StringBuilder();
				builder.Append("ModelState:")
					.Append($" {nameof(entry.Key)}={entry.Key}")
					.Append($", {nameof(entry.Value.RawValue)}.Type={entry.Value.RawValue.GetType()}")
					.Append($", {nameof(entry.Value.RawValue)}={entry.Value.GetRawValueAsString()}");
				_logger.LogInformation(builder.ToString());
			}

			return RedirectToAction(nameof(Form));
		}
	}
}
