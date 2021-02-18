using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedCacheWebApp.Controllers {
	public class SessionController : Controller {
		private const string _sessionKey = "session-sample";

		private ISession Session => HttpContext.Session;

		public IActionResult Get() {
			// セッションから取得
			var value = Session.GetString(_sessionKey);
			if (string.IsNullOrWhiteSpace(value)) {
				return Content("get error");
			}

			return Content($"get: {value}");
		}

		public IActionResult Set(string value) {
			if (string.IsNullOrEmpty(value)) {
				return Content("set error");
			}

			// セッションに設定
			Session.SetString(_sessionKey, $"{value}");

			return Content($"set: {value}");
		}

		public IActionResult Remove() {
			// セッションから削除
			Session.Remove(_sessionKey);

			return Content($"remove");
		}
	}

}
