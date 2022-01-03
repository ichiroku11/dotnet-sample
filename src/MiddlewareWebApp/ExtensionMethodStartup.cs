namespace MiddlewareWebApp;

public class ExtensionMethodStartup {
	public void ConfigureServices(IServiceCollection services) {
	}

	private void ConfigureSecond(IApplicationBuilder app) {
		app.Use(async (context, next) => {
			await context.Response.WriteAsync("Second: before\n");
			await next.Invoke();
			await context.Response.WriteAsync("Second: after\n");
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		// 最初に呼び出されるミドルウェア
		app.Use(async (context, next) => {
			context.Response.ContentType = "text/plain";
			await context.Response.WriteAsync("First: before\n");
			await next.Invoke();
			await context.Response.WriteAsync("First: after\n");
		});

		// 特定の条件のときにミドルウェアを呼び出す
		app.UseWhen(
			context => context.Request.Path.StartsWithSegments("/second"),
			ConfigureSecond);

		// 最後に呼び出されるミドルウェア
		app.Run(async context => {
			await context.Response.WriteAsync("Last\n");
		});
	}
}
