using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MiscWebApi {
	public class Startup {
		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public void ConfigureServices(IServiceCollection services) {
			services.AddControllers();

			services.Configure<RouteOptions>(options => {
				options.LowercaseQueryStrings = true;
				options.LowercaseUrls = true;
			});

			// Swagger
			services.AddOpenApiDocument(settings => {
				settings.PostProcess = document => {
					document.Info.Title = "Misc API";
					document.Info.Description = "ASP.NET Core Web API";
				};
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}

			// Swagger
			app.UseOpenApi(settings => {
			});
			app.UseSwaggerUi3(settings => {
			});

			app.UseRouting();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					"default",
					"{controller=Default}/{action=Index}/{id?}");
			});
		}
	}
}