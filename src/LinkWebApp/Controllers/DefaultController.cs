using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace LinkWebApp.Controllers;

public class DefaultController : Controller {
	private readonly LinkGenerator _linkGenerator;

	public DefaultController(LinkGenerator linkGenerator) {
		_linkGenerator = linkGenerator;
	}

	public IActionResult Generator() {
		// 参考
		// https://learn.microsoft.com/ja-jp/aspnet/core/fundamentals/routing?view=aspnetcore-7.0#url-generation-concepts
		// todo: それぞれoverloadを試す
		var content = new StringBuilder()
			// 絶対パス
			// /
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction(HttpContext))
			// 絶対パス
			// /
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction(HttpContext, controller: "Sample"))
			// 絶対パス
			// /sample
			.AppendLine("GetPathByAction:")
			.AppendLine(_linkGenerator.GetPathByAction("Index", "Sample"))
			// 絶対URL
			// https://localhost:xxx/sample
			.AppendLine("GetUriByAction:")
			.AppendLine(_linkGenerator.GetUriByAction(HttpContext, "Index", "Sample"))
			.ToString();
		return Content(content);
	}
}
