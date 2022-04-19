
using CustomFormatterWebApp.Formatters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => {
	options.InputFormatters.Insert(0, new Base64JsonInputFormatter());
	options.OutputFormatters.Insert(0, new Base64JsonOutputFormatter());
});

var app = builder.Build();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

// ユニットテストのため
// https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-6.0#basic-tests-with-the-default-webapplicationfactory
public partial class Program {
}
