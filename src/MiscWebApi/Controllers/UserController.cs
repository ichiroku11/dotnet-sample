using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiscWebApi.Models;

namespace MiscWebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "User")]
public class UserController : ControllerBase {
	[HttpGet("me")]
	public UserMeResponse Me() => new UserMeResponse("x", "xx");

}