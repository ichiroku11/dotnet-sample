namespace OpenApiWebApi;

public class Startup {
	public void ConfigureServices(IServiceCollection services) {
		services.AddControllers();

		services.Configure<RouteOptions>(options => {
			options.LowercaseQueryStrings = true;
			options.LowercaseUrls = true;
		});

		// OpenAPI/Swagger
		services.AddOpenApiDocument(settings => {
			settings.PostProcess = document => {
				document.Info.Title = "Sample API";
				document.Info.Description = "ASP.NET Core Web API";
			};
		});
	}

	public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
		if (env.IsDevelopment()) {
			app.UseDeveloperExceptionPage();
		}

		// OpenAPI/Swagger
		app.UseOpenApi(settings => {
		});
		app.UseSwaggerUi(settings => {
		});

		app.UseRouting();

		app.UseEndpoints(endpoints => {
			endpoints.MapControllers();
		});
	}
}
