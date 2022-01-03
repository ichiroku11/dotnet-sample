using Microsoft.AspNetCore.Mvc;

namespace ControllerWebApp.Controllers;

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
