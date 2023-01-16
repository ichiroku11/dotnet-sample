using Microsoft.AspNetCore.Mvc;

namespace MiscWebApi.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ConsumeController : ControllerBase {
	public class Sample {
		public string Value { get; init; } = "";
	}

	[HttpPost]
	public Sample Default(Sample model) => model;

	[HttpPost]
	[Consumes("application/json")]
	public Sample ApplicationJson(Sample model) => model;
}
