using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkWebApp.Controllers;

public class DefaultController(LinkGenerator linkGenerator, LinkParser linkParser) : Controller {
	private readonly LinkGenerator _linkGenerator = linkGenerator;
	private readonly LinkParser _linkParser = linkParser;

	public IActionResult Generator() {
		// 参考
		// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#url-generation-concepts
		var content = _linkGenerator.GetPathByAction(HttpContext,
			action: "Index",
			controller: "Sample") ?? "";

		return Content(content);
	}

	public IActionResult Parser() {
		var values = _linkParser.ParsePathByEndpointName("default", "/sample");

		return Json(values);
	}
}
