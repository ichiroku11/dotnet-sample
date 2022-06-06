using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2cWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ValueController : ControllerBase {
	[HttpGet]
	public IEnumerable<string> Get() => new[] { "a", "b", "c" };
}
