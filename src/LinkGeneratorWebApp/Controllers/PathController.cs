using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;

public class PathController : Controller {
	private readonly LinkGenerator _linkGenerator;

	public PathController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Self() {
		var path = _linkGenerator.GetPathByAction(HttpContext);
		return Content(path ?? "");
	}
}
