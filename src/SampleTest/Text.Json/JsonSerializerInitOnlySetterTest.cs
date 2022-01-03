using System.Text.Json;
using Xunit;

namespace SampleTest.Text.Json;

public class JsonSerializerInitOnlySetterTest {
	// init専用セッターのサンプル
	private class SampleWithInitOnlySetter {
		public int Number { get; init; }
		public string Text { get; init; } = "";
	}

	[Fact]
	public void Deserialize_init専用セッターのプロパティに対してデシリアライズできる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<SampleWithInitOnlySetter>(@"{""number"":1,""text"":""Abc""}", options)!;

		// Assert
		Assert.Equal(1, sample.Number);
		Assert.Equal("Abc", sample.Text);
	}
}
