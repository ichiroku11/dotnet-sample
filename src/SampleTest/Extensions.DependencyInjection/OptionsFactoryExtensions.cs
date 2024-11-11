using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

internal static class OptionsFactoryExtensions {
	public static TOptions CreateDefault<TOptions>(this IOptionsFactory<TOptions> factory) where TOptions : class
		// SampleTest.Extensions.Optionsを用意したらCS0234が発生するため
		//=> factory.Create(Options.DefaultName);
		=> factory.Create(Microsoft.Extensions.Options.Options.DefaultName);
}
