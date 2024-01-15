namespace DistributedCacheWebApp;

public class Startup(IConfiguration config) {
	private readonly IConfiguration _config = config;

	public void ConfigureServices(IServiceCollection services) {
		// セッションのサービスを追加
		services.AddSession(options => {
			// セッションクッキーの名前を変える
			options.Cookie.Name = "session";
		});

		// SQL Server分散キャッシュのサービスを追加
		services.AddDistributedSqlServerCache(options => {
			options.ConnectionString = _config.GetConnectionString("Cache");
			options.SchemaName = "dbo";
			options.TableName = "AppCache";
		});

		// コントローラ
		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		// セッション
		app.UseSession();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller}/{action=Get}/{value?}");
		});
	}
}
