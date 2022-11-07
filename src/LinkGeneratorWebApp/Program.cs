var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();
app.UseEndpoints(endpoints => {
	endpoints.MapControllerRoute(
		"default",
		"{controller=Home}/{action=Index}/{id?}");
});

app.Run();
