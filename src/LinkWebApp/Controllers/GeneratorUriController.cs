using Microsoft.AspNetCore.Mvc;

namespace LinkWebApp.Controllers;
public class GeneratorUriController(LinkGenerator linkGenerator) : Controller {
	private readonly LinkGenerator _linkGenerator = linkGenerator;

	public IActionResult Self() {
		// このアクションへのURLを生成する
		var path = _linkGenerator.GetUriByAction(HttpContext);
		return Content(path ?? "");
	}

	public IActionResult AnotherControllerAction() {
		// このアクションとは別のコントローラーへのURLを生成する
		var path = _linkGenerator.GetUriByAction(
			action: "Index",
			controller: "Another",
			values: default,
			scheme: "https",
			host: new HostString("localhost"));
		return Content(path ?? "");
	}
}
