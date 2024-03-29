using Microsoft.AspNetCore.Mvc;

namespace LinkWebApp.Controllers;

public class GeneratorPathController(LinkGenerator linkGenerator) : Controller {
	private readonly LinkGenerator _linkGenerator = linkGenerator;

	public IActionResult Self() {
		// このアクションへのURLを生成する
		var path = _linkGenerator.GetPathByAction(HttpContext);
		return Content(path ?? "");
	}

	public IActionResult SelfWithoutHttpContext() {
		// HttpContextを使わずにこのアクションへのURLを生成する
		var path = _linkGenerator.GetPathByAction(
			action: nameof(SelfWithoutHttpContext),
			controller: "GeneratorPath");
		return Content(path ?? "");
	}

	public IActionResult AnotherAction() {
		// このアクションとは別のアクションへのURLを生成する
		// controllerは指定していないのでHttpContextから補完される様子
		var path = _linkGenerator.GetPathByAction(
			HttpContext,
			action: "Another");
		return Content(path ?? "");
	}

	public IActionResult AnotherController() {
		// このアクションとは別のコントローラーへのURLを生成する
		// actionは指定していないのでHttpContextから補完される様子
		var path = _linkGenerator.GetPathByAction(
			HttpContext,
			controller: "Another");
		return Content(path ?? "");
	}

	public IActionResult AnotherControllerAction() {
		// このアクションとは別のコントローラーへのURLを生成する
		var path = _linkGenerator.GetPathByAction(
			HttpContext,
			action: "Index",
			controller: "Another");
		return Content(path ?? "");
	}

	public IActionResult AnotherControllerActionWithoutHttpContext() {
		// このアクションとは別のコントローラーへのURLを生成する
		var path = _linkGenerator.GetPathByAction(
			action: "Index",
			controller: "Another");
		return Content(path ?? "");
	}
}
