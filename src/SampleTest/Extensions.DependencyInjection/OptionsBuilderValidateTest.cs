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
	public void Validate_IValidateOptionsが登録されていることを確認する() {
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
}
