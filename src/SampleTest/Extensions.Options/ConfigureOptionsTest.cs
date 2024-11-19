using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.Options;

public class ConfigureOptionsTest {
	private class SampleOptions {
		public int Value { get; set; }
	}

	[Fact]
	public void Configure_コンストラクターで指定したアクションメソッドが呼び出されることを確認する() {
		// Arrange
		var options = new SampleOptions();
		Assert.Equal(0, options.Value);

		var configure = new ConfigureOptions<SampleOptions>(options => {
			options.Value = 1;
		});

		// Act
		configure.Configure(options);

		// Assert
		Assert.Equal(1, options.Value);
	}
}
