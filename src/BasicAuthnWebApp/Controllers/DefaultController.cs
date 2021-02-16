using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicAuthnWebApp.Controllers {
	public class DefaultController : Controller {
		// 認証不要な（匿名でアクセスできる）アクション
		[AllowAnonymous]
		public IActionResult AllowAnonymous() => Content(nameof(AllowAnonymous));

		// 認証が必要なアクション
		public IActionResult RequireAuthenticated() => Content(nameof(RequireAuthenticated));
	}
}
