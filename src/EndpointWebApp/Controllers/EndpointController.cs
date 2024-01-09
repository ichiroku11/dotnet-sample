using System.Text;

using Microsoft.AspNetCore.Mvc;

namespace EndpointWebApp.Controllers;

public class EndpointController(EndpointDataSource dataSource) : Controller {
	private readonly EndpointDataSource _dataSource = dataSource;

	[Route("~/endpoints")]
	public IActionResult Index() {
		var builder = new StringBuilder();

		foreach (var endpoint in _dataSource.Endpoints) {
			builder.AppendLine(endpoint.DisplayName);

			foreach (var metadata in endpoint.Metadata) {
				builder.AppendLine($"\t{metadata}");
			}
			builder.AppendLine();
		}

		return Content(builder.ToString());
	}
}
