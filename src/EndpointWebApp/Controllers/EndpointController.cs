using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EndpointWebApp.Controllers;

public class EndpointController : Controller {
	private readonly EndpointDataSource _dataSource;

	public EndpointController(EndpointDataSource dataSource) {
		_dataSource = dataSource;
	}

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
