using AzureAppInsightsWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppInsightsWebApp.Controllers;

public class HomeController : Controller {
	public IActionResult Index() {
		return View();
	}

	public async Task<IActionResult> SqlAsync() {
		using var dbContext = HttpContext.RequestServices.GetRequiredService<AppDbContext>();

		// 単にクエリをログに残したいだけ
		// 影響を受けた行はないのでresult = -1
		var result = await dbContext.Database.ExecuteSqlRawAsync("select 1");
		return Content(result.ToString());
	}
}
