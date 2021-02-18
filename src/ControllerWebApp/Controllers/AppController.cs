using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ControllerWebApp.Controllers {
	// abstractを指定しないとアクセスできるコントローラになってしまう
	public abstract class AppController : Controller {
		// NonActionを指定しないとActionメソッドになる
		[NonAction]
		public IActionResult Base() => Content(nameof(Base));

		// protectedなメソッドはActionメソッドにならない
		protected string ProtectedMethod() => nameof(ProtectedMethod);
		// protectedなGetterプロパティはActionメソッドにならない
		protected string ProtectedGetterProperty => nameof(ProtectedGetterProperty);
	}
}
