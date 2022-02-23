using System.Globalization;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options => {
	options.SetDefaultCulture("ja-JP");
	// ja-JPをen-USでするなら
	//options.SetDefaultCulture("en-US");
});

var app = builder.Build();

app.UseRequestLocalization();

app.MapGet("/", () => {
	var content = new StringBuilder()
		.AppendLine($"{nameof(CultureInfo.CurrentCulture)}: {CultureInfo.CurrentCulture}")
		.AppendLine($"{nameof(CultureInfo.CurrentUICulture)}: {CultureInfo.CurrentUICulture}")
		.ToString();
	return content;
});

app.Run();
