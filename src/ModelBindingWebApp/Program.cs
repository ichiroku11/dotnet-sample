using ModelBindingWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services
	.AddControllersWithViews(options => {
		options.ModelBinderProviders.Insert(0, new GeometryModelBinderProvider());
	})
	.AddViewOptions(options => {
		// クライアント側バリーデーションを無効にする
		options.HtmlHelperOptions.ClientValidationEnabled = false;
	});

// セッションを使うなら
/*
services
	.AddControllersWithViews()
	// TempDataはセッションを使う
	.AddSessionStateTempDataProvider();

services.AddSession();
*/

services.Configure<RouteOptions>(options => {
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

// セッションを使うなら
//app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

app.Run();

// 統合テストのため
public partial class Program {
}
