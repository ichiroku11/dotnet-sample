using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class OptionsBuilderValidateTest {
	private class SampleOptions {
		public int Value { get; set; }
	}

	private readonly ITestOutputHelper _output;

	public OptionsBuilderValidateTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Validate_IValidateOptionsが登録されている() {
		var services = new ServiceCollection();
		var validated = false;

		// Act
		services
			.AddOptions<SampleOptions>()
			.Validate(options => {
				validated = true;
				return true;
			});

		// Assert
		// IValidateOptionsが登録されるが
		Assert.Single(services, service => service.ServiceType == typeof(IValidateOptions<SampleOptions>));
		// 登録しただけでは呼び出されない
		Assert.False(validated);
	}

	[Fact]
	public void Validate_OptionsFactory経由でオプションを生成するとValidateは呼び出される() {
		var services = new ServiceCollection();
		var validated = false;

		services
			.AddOptions<SampleOptions>()
			.Validate(options => {
				_output.WriteLine(nameof(validated));
				validated = true;
				return true;
			});
		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		Assert.False(validated);
		var options = factory.Create(Options.DefaultName);

		// Assert
		Assert.NotNull(options);
		Assert.True(validated);
	}

	[Fact]
	public void Validate_オプションの検証エラーになるとOptionsValidationExceptionが発生する() {
		var services = new ServiceCollection();
		services
			.AddOptions<SampleOptions>()
			.Validate(options => {
				// バリデーションエラーとする
				return false;
			});
		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		// Assert
		var exception = Assert.Throws<OptionsValidationException>(() => {
			factory.Create(Options.DefaultName);
		});

		_output.WriteLine(exception.Message);
	}
}
