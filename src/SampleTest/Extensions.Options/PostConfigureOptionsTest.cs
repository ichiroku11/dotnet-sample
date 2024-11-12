using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.Options;

public class PostConfigureOptionsTest {
	private class SampleOptions {
		public int Value { get; set; }
	}

	[Fact]
	public void Configure_コンストラクターで指定したアクションメソッドが呼び出されることを確認する() {
		// Arrange
		var options = new SampleOptions();
		Assert.Equal(0, options.Value);

		var configure = new PostConfigureOptions<SampleOptions>(
			null,
			options => {
				options.Value = 1;
			});

		// Act
		configure.PostConfigure(null, options);

		// Assert
		Assert.Equal(1, options.Value);
	}
}
