using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiscWebApi.Controllers;

// ApiExplorerでは取得できない
public class DefaultController : Controller {
	public IActionResult Index() => View();
	public IActionResult Test() => Json(new { x = 1 });
}
