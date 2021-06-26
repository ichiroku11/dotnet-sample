using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp.Controllers {
	[Authorize]
	public class HomeController : Controller {
		[AllowAnonymous]
		public IActionResult Index() => View();

		[AllowAnonymous]
		public IActionResult Option() => View();

		public async Task<IActionResult> ClaimAsync() {
			// todo:
			//var result = await HttpContext.AuthenticateAsync();
			//ViewBag.Properties = result.Properties;
			ViewBag.Properties = new AuthenticationProperties();

			await Task.CompletedTask;

			return View();
		}
	}
}
