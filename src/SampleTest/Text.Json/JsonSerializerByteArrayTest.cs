using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonSerializerByteArrayTest {
	private static readonly JsonSerializerOptions _options = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase
	};

	[Fact]
	public void Serialize_バイト配列をシリアライズするとBase64にエンコードされる() {
		// Arrange
		var bytes = new byte[] { 0b_1111_1000 };

		// Act
		var json = JsonSerializer.Serialize(bytes, _options);

		// Assert
		Assert.Equal(@"""+A==""", json);
	}

	// バイト配列のプロパティを持つ
	private record Sample(byte[] Values);

	[Fact]
	public void Serialize_バイト配列のプロパティを持つオブジェクトをシリアライズするとBase64にエンコードされる() {
		// Arrange
		var sample = new Sample(new byte[] { 0b_1111_1000 });

		// Act
		var json = JsonSerializer.Serialize(sample, _options);

		// Assert
		Assert.Equal(@"{""values"":""+A==""}", json);
	}
}
