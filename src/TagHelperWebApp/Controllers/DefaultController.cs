using System.Text;
using Microsoft.AspNetCore.Mvc;
using TagHelperWebApp.Models;

namespace TagHelperWebApp.Controllers;

public class DefaultController(ILogger<DefaultController> logger) : Controller {
	private readonly ILogger _logger = logger;

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
				.Append($", {nameof(entry.Value.RawValue)}.Type={entry.Value.RawValue?.GetType()}")
				.Append($", {nameof(entry.Value.RawValue)}={entry.Value.GetRawValueAsString()}");
			_logger.LogInformation("{modelState}", builder.ToString());
		}

		return RedirectToAction(nameof(Form));
	}

	public IActionResult ByteArray() => View(new ByteArrayCommand { Bytes = [0x01, 0x02] });
}
