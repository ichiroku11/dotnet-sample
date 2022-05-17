var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllers();

var config = builder.Configuration;
var clientId = config["AzureAdB2C:ClientId"];
Console.WriteLine(clientId);

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => {
	endpoints.MapControllerRoute(
		"default",
		"{controller=Default}/{action=Index}/{id?}");
});

app.Run();

// https://docs.microsoft.com/ja-jp/azure/active-directory-b2c/enable-authentication-web-api?tabs=csharpclient
// https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2

