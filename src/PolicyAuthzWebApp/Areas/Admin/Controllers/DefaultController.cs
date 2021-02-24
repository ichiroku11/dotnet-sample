using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PolicyAuthzWebApp.Areas.Admin.Controllers {
	[Area("Admin")]
	public class DefaultController : Controller {
		public IActionResult Index() {
			return Content($"~/admin/default/{nameof(Index).ToLower()}");
		}
	}
}
