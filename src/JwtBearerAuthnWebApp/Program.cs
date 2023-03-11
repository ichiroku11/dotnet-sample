using System.Security.Claims;
using System.Text;

// 参考
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddAuthorization();
services.AddAuthentication("Bearer").AddJwtBearer();


var app = builder.Build();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/secret", handler)
	// 認証が必要
	.RequireAuthorization();

app.MapGet("/secret2", handler)
	// 認証かつスコープが必要
	.RequireAuthorization(policyBuilder => policyBuilder.RequireClaim("scope", "api:read", "api:write"));

app.Run();


static string handler(ClaimsPrincipal user) {
	var content = new StringBuilder();

	foreach (var claim in user.Claims) {
		content.AppendLine($"{claim.Type}={claim.Value}");
	}

	return content.ToString();
}
