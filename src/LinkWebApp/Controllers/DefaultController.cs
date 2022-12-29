using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkWebApp.Controllers;

public class DefaultController : Controller {
	private readonly LinkGenerator _linkGenerator;
	private readonly LinkParser _linkParser;

	public DefaultController(LinkGenerator linkGenerator, LinkParser linkParser) {
		_linkGenerator = linkGenerator;
		_linkParser = linkParser;
	}

	public IActionResult Generator() {
		// 参考
		// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#url-generation-concepts
		// todo: それぞれoverloadを試す
		var content = new StringBuilder()
			// 絶対パス
			// /
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction(HttpContext))
			// 絶対パス
			// /
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction(HttpContext, controller: "Sample"))
			// 絶対パス
			// /sample
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction("Index", "Sample"))
			// 絶対URL
			// https://localhost:xxx/sample
			.AppendLine("GetUriByAction:")
			.AppendLine(_linkGenerator.GetUriByAction(HttpContext, "Index", "Sample"))
			.ToString();
		return Content(content);
	}

	public IActionResult Parser() {
		var values = _linkParser.ParsePathByEndpointName("default", "/sample");

		return Json(values);
	}
}
