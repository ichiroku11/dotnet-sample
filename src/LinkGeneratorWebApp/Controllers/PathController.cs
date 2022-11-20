using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;

public class PathController : Controller {
	private readonly LinkGenerator _linkGenerator;

	public PathController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Self() {
		// このアクションへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext);
		return Content(path ?? "");
	}

	public IActionResult OtherAction() {
		// このアクションとは別のアクションへのURLを生成する
		// controllerは指定していないのでHttpContextから補完される様子
		var path = _linkGenerator.GetPathByAction(HttpContext, action: "Other");
		return Content(path ?? "");
	}

	public IActionResult OtherController() {
		// このアクションとは別のコントローラーへのURLを生成する
		// actionは指定していないのでHttpContextから補完される様子
		var path = _linkGenerator.GetPathByAction(HttpContext, controller: "Other");
		return Content(path ?? "");
	}

	public IActionResult OtherControllerAction() {
		// このアクションとは別のコントローラーへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext, action: "Index", controller: "Other");
		return Content(path ?? "");
	}
}
