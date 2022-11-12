using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkGeneratorWebApp.Controllers;
public class DefaultController : Controller {

	private readonly LinkGenerator _linkGenerator;

	public DefaultController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Index() {
		var content = new StringBuilder()
			// 絶対パス
			// /sample
			.AppendLine($"GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction("Index", "Sample") ?? "")
			// 絶対URL
			// https://localhost:xxx/sample
			.AppendLine($"GetUriByAction:")
			.AppendLine(_linkGenerator.GetUriByAction(HttpContext, "Index", "Sample") ?? "")
			.ToString();
		return Content(content);
	}
}
