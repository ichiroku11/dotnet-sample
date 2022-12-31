using Microsoft.AspNetCore.Mvc;

namespace LinkWebApp.Controllers;

public class ParserPathController : Controller {
	private readonly LinkParser _linkParser;

	public ParserPathController(LinkParser linkParser) {
		_linkParser = linkParser;
	}

	public IActionResult Default() {
		// defaultのルート
		var values = _linkParser.ParsePathByEndpointName("default", "/sample/index/1");

		return Json(values);
	}
}
