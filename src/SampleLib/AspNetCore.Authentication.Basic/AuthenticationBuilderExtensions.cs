using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace SampleLib.AspNetCore.Authentication.Basic;

public static class AuthenticationBuilderExtensions {
	public static AuthenticationBuilder AddBasic(this AuthenticationBuilder builder)
		=> builder.AddBasic(
			BasicAuthenticationDefaults.AuthenticationScheme,
			_ => { });

	public static AuthenticationBuilder AddBasic(
		this AuthenticationBuilder builder,
		Action<BasicAuthenticationOptions> configureOptions)
		=> builder.AddBasic(
			BasicAuthenticationDefaults.AuthenticationScheme,
			configureOptions);

	public static AuthenticationBuilder AddBasic(
		this AuthenticationBuilder builder,
		string authenticationScheme,
		Action<BasicAuthenticationOptions> configureOptions) {

		builder.Services.TryAddEnumerable(
			ServiceDescriptor.Singleton<IPostConfigureOptions<BasicAuthenticationOptions>, BasicAuthenticationPostConfigureOptions>());

		return builder.AddScheme<BasicAuthenticationOptions, BasicAuthenticationHandler>(
			authenticationScheme: authenticationScheme,
			displayName: null,
			configureOptions: configureOptions);
	}
}
