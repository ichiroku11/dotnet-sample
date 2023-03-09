using System.Security.Claims;

// 参考
// https://learn.microsoft.com/en-us/aspnet/core/security/authentication/jwt-authn?view=aspnetcore-7.0&tabs=windows

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddAuthorization();
services.AddAuthentication("Bearer").AddJwtBearer();


var app = builder.Build();

app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapGet("/secret", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}. My secret")
	.RequireAuthorization();

app.MapGet("/secret2", () => "This is a different secret!")
	.RequireAuthorization(policyBuilder => policyBuilder.RequireClaim("scope", "myapi:secrets"));

app.Run();
