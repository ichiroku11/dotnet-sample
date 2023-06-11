using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

// 参考
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/MinimalOpenIdConnectSample/Program.cs
// https://github.com/dotnet/aspnetcore/blob/main/src/Security/Authentication/OpenIdConnect/samples/OpenIdConnectSample/Startup.cs

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

services
	.AddAuthentication(options => {
		options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
	})
	.AddCookie(options => {
		// todo:
	})
	.AddOpenIdConnect(options => {
		config.GetSection("Line").Bind(options);

		if (string.IsNullOrWhiteSpace(options.ClientSecret)) {
			throw new InvalidProgramException();
		}

		// 下記より
		// https://developers.line.biz/ja/docs/line-login/verify-id-token/#signature
		// OpenIDプロバイダーの情報
		// todo: LineDefaultsか
		options.MetadataAddress = "https://access.line.me/.well-known/openid-configuration";

		// response_typeはcode
		// https://developers.line.biz/ja/docs/line-login/integrate-line-login/#applying-for-email-permission
		options.ResponseType = OpenIdConnectResponseType.Code;

		// 署名を検証する鍵はクライアントシークレット
		// ウェブログインにおける署名はHS256で、鍵はチャネルシークレット
		// 下記より
		// https://developers.line.biz/ja/docs/line-login/verify-id-token/#header
		// https://developers.line.biz/ja/docs/line-login/verify-id-token/#signature
		options.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.ClientSecret));

		options.Events = new OpenIdConnectEvents {
			OnTokenResponseReceived = async (context) => {
				// todo:
				await Task.CompletedTask;
			}
		};
	});

services.AddAuthorization();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");
app.MapGet("/protected", (ClaimsPrincipal user) => $"Hello {user.Identity?.Name}!")
	.RequireAuthorization();

app.Run();
