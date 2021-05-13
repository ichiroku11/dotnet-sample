using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAdB2cWebApp.Controllers {
	public class HomeController : Controller {
		public IActionResult Index() {
			return View();
		}

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
