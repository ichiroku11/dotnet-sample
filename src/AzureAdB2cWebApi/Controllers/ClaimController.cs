using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AzureAdB2cWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ClaimController : ControllerBase {
	[HttpGet]
	public object Get() => new { User.Identity?.IsAuthenticated };

}
