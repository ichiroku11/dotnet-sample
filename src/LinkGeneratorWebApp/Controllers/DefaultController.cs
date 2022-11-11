using Microsoft.AspNetCore.Mvc;

namespace LinkGeneratorWebApp.Controllers;
public class DefaultController : Controller {

	private readonly LinkGenerator _linkGenerator;

	public DefaultController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Index() {
		var path = _linkGenerator.GetPathByAction("Index", "Default") ?? "";
		return Content(path);
	}
}
