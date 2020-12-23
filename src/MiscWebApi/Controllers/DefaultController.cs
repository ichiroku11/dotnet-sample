using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiscWebApi.Controllers {
	// OpenAPIには含まれない
	public class DefaultController : Controller {
		public IActionResult Index() => View();

		public IActionResult Test() => Json(new { x = 1 });
	}
}
