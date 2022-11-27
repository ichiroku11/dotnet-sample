using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;
public class UriController : Controller {
	private readonly LinkGenerator _linkGenerator;

	public UriController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

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
