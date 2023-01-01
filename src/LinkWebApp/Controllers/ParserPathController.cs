using Microsoft.AspNetCore.Mvc;

namespace LinkWebApp.Controllers;

public class ParserPathController : Controller {
	private readonly LinkParser _linkParser;

	public ParserPathController(LinkParser linkParser) {
		_linkParser = linkParser;
	}

	// defaultのルートでパースする
	public IActionResult Default() {
		var values = _linkParser.ParsePathByEndpointName("default", "/sample/index/1");

		return Json(values);
	}

	// Route属性で指定したルートでパースする
	public IActionResult AnotherRoute() {
		var values = _linkParser.ParsePathByEndpointName("another", "/another/a-b-c");

		return Json(values);
	}

	[Route("~/another/{x}-{y}-{z}", Name = "another")]
	public IActionResult Another() {
		return new EmptyResult();
	}
}
