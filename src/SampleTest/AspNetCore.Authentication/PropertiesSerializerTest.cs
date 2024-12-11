using Microsoft.AspNetCore.Authentication;

namespace SampleTest.AspNetCore.Authentication;

public class PropertiesSerializerTest {
	[Fact]
	public void Serialize_Deserialize_動きを確認する() {
		// Arrange
		var expected = new AuthenticationProperties {
			RedirectUri = "redirect-uri",
		};
		var serializer = new PropertiesSerializer();

		// Act
		var serializedData = serializer.Serialize(expected);
		var actual = serializer.Deserialize(serializedData);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(expected.RedirectUri, actual.RedirectUri);
	}

	[Fact]
	public void Serialize_Parametersはシリアライズされない() {
		// Arrange
		var properties = new AuthenticationProperties {
		};
		properties.Parameters.Add("key", "value");

		var serializer = new PropertiesSerializer();

		// Act
		var serializedData = serializer.Serialize(properties);
		var actual = serializer.Deserialize(serializedData);

		// Assert
		Assert.NotNull(actual);
		Assert.Empty(actual.Parameters);
	}
}
