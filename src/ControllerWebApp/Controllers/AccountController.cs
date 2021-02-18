using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ControllerWebApp.Controllers {
	public class AccountController : AppController {
		public IActionResult Index() => Content($"Account.{nameof(Index)}");
	}
}
