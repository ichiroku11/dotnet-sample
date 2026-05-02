var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
var services = builder.Services;

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

var app = builder.Build();

if (env.IsDevelopment()) {
	app.UseDeveloperExceptionPage();
}

// OpenAPI/Swagger
app.UseOpenApi(settings => {
});
app.UseSwaggerUi(settings => {
});

app.UseRouting();

app.MapControllers();

app.Run();
