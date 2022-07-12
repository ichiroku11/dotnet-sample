using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

public class JsonSerializerOptionsTest {
	private readonly ITestOutputHelper _output;

	public JsonSerializerOptionsTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void DefaultIgnoreCondition_Alwaysを設定するとArgumentExceptionがスローされる() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<ArgumentException>(() => {
			var options = new JsonSerializerOptions {
				DefaultIgnoreCondition = JsonIgnoreCondition.Always
			};
		});
	}
}
