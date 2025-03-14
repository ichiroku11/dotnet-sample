using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace SampleTest.Extensions.DependencyInjection;

public class OptionsBuilderValidateTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class SampleOptions {
		[Range(1, 10)]
		public int Value { get; set; }
	}

	[Fact]
	public void Validate_IValidateOptionsが登録されている() {
		// Arrange
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
		// Arrange
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
		var options = factory.CreateDefault();

		// Assert
		Assert.NotNull(options);
		Assert.True(validated);
	}

	[Fact]
	public void Validate_オプションの検証エラーになるとOptionsValidationExceptionが発生する() {
		// Arrange
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
		var recorded = Record.Exception(() => {
			factory.CreateDefault();
		});

		// Assert
		var exception = Assert.IsType<OptionsValidationException>(recorded);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void ValidateDataAnnotations_属性による検証エラーでOptionsValidationExceptionが発生する() {
		// Arrange
		var services = new ServiceCollection();
		services
			.AddOptions<SampleOptions>()
			.ValidateDataAnnotations();
		var provider = services.BuildServiceProvider();
		var factory = provider.GetRequiredService<IOptionsFactory<SampleOptions>>();

		// Act
		var recorded = Record.Exception(() => {
			factory.CreateDefault();
		});

		// Assert
		var exception = Assert.IsType<OptionsValidationException>(recorded);

		// バリデーション失敗のメッセージ
		var failure = Assert.Single(exception.Failures);
		_output.WriteLine(failure);

		// CS0234
		//Assert.Equal(Options.DefaultName, exception.OptionsName);
		Assert.Empty(exception.OptionsName);
		Assert.Equal(typeof(SampleOptions), exception.OptionsType);
	}

	[Fact]
	public void ValidateOnStart_プログラム起動時にランタイムから呼び出されて検証される() {
		// Arrange
		var services = new ServiceCollection();
		services
			.AddOptions<SampleOptions>()
			.ValidateDataAnnotations()
			.ValidateOnStart();
		var provider = services.BuildServiceProvider();

		// Act
		var recorded = Record.Exception(() => {
			// ASP.NET Coreでは起動時に呼ばれるらしい
			provider.GetRequiredService<IStartupValidator>().Validate();
		});

		// Assert
		var exception = Assert.IsType<OptionsValidationException>(recorded);

		// バリデーション失敗のメッセージ
		var failure = Assert.Single(exception.Failures);
		_output.WriteLine(failure);

		// CS0234
		//Assert.Equal(Options.DefaultName, exception.OptionsName);
		Assert.Empty(exception.OptionsName);
		Assert.Equal(typeof(SampleOptions), exception.OptionsType);
	}
}
