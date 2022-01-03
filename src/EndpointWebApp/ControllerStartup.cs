namespace EndpointWebApp;

// MVC、コントローラを使った場合のEndpointとMetadataを確認するStartup
public class ControllerStartup {
	public void ConfigureServices(IServiceCollection services) {
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
				pattern: "{controller=Default}/{action=Index}/{value?}");
		});
	}
}
