using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;

public class PathController : Controller {
	private readonly LinkGenerator _linkGenerator;

	public PathController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Self() {
		// HttpContextを指定
		// このアクションへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext);
		return Content(path ?? "");
	}

	public IActionResult OtherAction() {
		// HttpContextを指定
		// このアクションとは別のアクションへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext, action: "Other");
		return Content(path ?? "");
	}

	public IActionResult OtherControllerAction() {
		// HttpContextを指定
		// このアクションとは別のコントローラーへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext, action: "Index", controller: "Other");
		return Content(path ?? "");
	}
}
