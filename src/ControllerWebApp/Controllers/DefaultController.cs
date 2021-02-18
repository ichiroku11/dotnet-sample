using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;

namespace ControllerWebApp.Controllers {
	public class DefaultController : AppController {
		private readonly ApplicationPartManager _manager;

		public DefaultController(ApplicationPartManager manager) {
			_manager = manager;
		}

		// privateなメソッドはActionメソッドにならない
		private string PrivateMethod() => nameof(PrivateMethod);
		// privateなGetterプロパティはActionメソッドにならない
		private string PrivateGetterProperty => nameof(PrivateGetterProperty);

		// publicなメソッドはActionメソッドになる
		public string PublicMethod() => nameof(PublicMethod);
		// publicなGetterプロパティはActionメソッドにならない
		public string PublicGetterProperty => nameof(PublicGetterProperty);

		public IActionResult Index() => Content($"Default.{nameof(Index)}");

		[NonAction]
		public IActionResult NonAction() => Content($"Default.{nameof(NonAction)}");

		[HttpGet]
		public IActionResult HttpGet() => Content($"Default.{nameof(HttpGet)}");
		[HttpPost]
		public IActionResult HttpPost() => Content($"Default.{nameof(HttpPost)}");

		public IActionResult Controllers() {
			// コントローラ一覧を取得
			var feature = new ControllerFeature();
			_manager.PopulateFeature(feature);

			var content = new StringBuilder();
			foreach (var controller in feature.Controllers) {
				content.AppendLine(controller.Name);
			}

			return Content(content.ToString());
		}

		public IActionResult ControllerActions() {
			// コントローラ一覧を取得
			var feature = new ControllerFeature();
			_manager.PopulateFeature(feature);

			// アクション一覧
			var actions = feature.Controllers
				.SelectMany(controller => controller.DeclaredMethods)
				// publicなメソッド
				.Where(method => method.IsPublic)
				// 特殊な処理をされるメソッドではない
				.Where(method => !method.IsSpecialName)
				// NonAction属性が指定されていない
				.Where(method => !method.GetCustomAttributes<NonActionAttribute>().Any())
				// 自動生成されたメソッドではない
				.Where(method => !method.GetCustomAttributes<CompilerGeneratedAttribute>().Any());

			var content = new StringBuilder();
			foreach (var action in actions) {
				var controller = action.DeclaringType;
				var area = action.GetCustomAttribute<AreaAttribute>()?.RouteValue
					?? controller.GetCustomAttribute<AreaAttribute>()?.RouteValue;
				var httpMethods = action.GetCustomAttribute<HttpMethodAttribute>()?.HttpMethods
					?? Enumerable.Empty<string>();

				content.AppendLine($"{area}, {controller.Name}, {action.Name}, {string.Join("/", httpMethods)}");
			}

			return Content(content.ToString());
		}
	}
}
