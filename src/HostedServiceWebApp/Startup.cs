using HostedServiceWebApp.Services;

namespace HostedServiceWebApp;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		// スケジュールに従って定期的な処理を実行するサンプル
		services
			.AddHostedService<SampleScheduleService>();

		// AddScopedで追加されたサービスをIHostedServiceから呼び出すサンプル
		services
			.AddScoped<SampleScopedService>()
			.AddHostedService<SampleBackgroundServiceWithScopedService>();

		services
			// BackgroundService継承したサンプル
			.AddHostedService<SampleBackgroundService>()
			// IHostedServiceを実装したサンプル
			.AddHostedService<SampleHostedService>();
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapGet("/", async context => {
				await context.Response.WriteAsync("Hello World!");
			});
		});
	}
}
