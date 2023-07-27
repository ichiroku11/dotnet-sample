using Microsoft.Extensions.Configuration;
using System.Text;

namespace SampleTest.Extensions.Configuration;

public class JsonConfigurationExtensionsTest {

	private class SampleOptions {
		public int Value { get; init; }
	}

	[Fact]
	public void AddJsonStream_JSONの構成を試す() {
		// Arrange
		var json = @"{""x"":{""value"":1}}";
		using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

		// Act
		var config = new ConfigurationBuilder()
			.AddJsonStream(stream)
			.Build();
		var options = config.GetSection("x").Get<SampleOptions>();

		// Assert
		Assert.NotNull(options);
		Assert.Equal(1, options.Value);
	}
}
