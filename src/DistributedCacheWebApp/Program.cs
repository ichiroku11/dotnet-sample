var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var config = builder.Configuration;
var services = builder.Services;

// セッションのサービスを追加
services.AddSession(options => {
	// セッションクッキーの名前を変える
	options.Cookie.Name = "session";
});

// SQL Server分散キャッシュのサービスを追加
services.AddDistributedSqlServerCache(options => {
	options.ConnectionString = config.GetConnectionString("Cache");
	options.SchemaName = "dbo";
	options.TableName = "AppCache";
});

// コントローラ
services.AddControllers();

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

app.UseRouting();

// セッション
app.UseSession();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller}/{action=Get}/{value?}");

app.Run();
