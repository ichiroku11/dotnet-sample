var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

services.AddControllers();
services.Configure<RouteOptions>(options => {
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// これのほうがよさげな
//app.UseStatusCodePagesWithReExecute("/error/{0}");
//app.UseStatusCodePagesWithReExecute("/error/notfound");

app.UseRouting();

// デフォルトルート
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Default}/{action=Index}/{id?}");

// フォールバックルート
// 未定義のアクション（URL）にアクセスされると
// 下記コントローラ・アクションが呼び出される様子
app.MapFallbackToController(
	controller: "Error",
	action: "NotFound");

app.Run();
