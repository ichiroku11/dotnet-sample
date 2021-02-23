using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EndpointWebApp.Controllers {
	public class DefaultController : Controller {
		public IActionResult Index() {
			return Content(nameof(Index));
		}

		[Authorize]
		public IActionResult Test() {
			return Content(nameof(Test));
		}
	}
}
