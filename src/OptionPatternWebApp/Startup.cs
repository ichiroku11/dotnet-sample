namespace OptionPatternWebApp;

public class Startup(IConfiguration config) {
	private readonly IConfiguration _config = config;

	public void ConfigureServices(IServiceCollection services) {
		// 設定をクラスにバインドできるようにする
		services.Configure<SampleOptions>(_config.GetSection("App:Sample"));
		// 別の方法
		//services.Configure<SampleOptions>(_config.GetSection("App").GetSection("Sample"));
		services.Configure<SampleOptionsMonitor>(_config.GetSection("App:SampleMonitor"));

		services.AddControllers();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllerRoute(
				name: "default",
				pattern: "{controller=Default}/{action=Index}/{id?}");
		});
	}
}
