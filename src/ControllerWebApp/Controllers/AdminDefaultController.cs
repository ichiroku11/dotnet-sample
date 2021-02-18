using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerWebApp.Controllers {
	[Area("Admin")]
	public class AdminDefaultController : AppController {
		public IActionResult Index() => Content($"AdminDefault.{nameof(Index)}");
	}
}
