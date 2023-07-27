
using Microsoft.Extensions.Configuration;

namespace SampleTest.Extensions.Configuration;

public class ConfigurationSectionTest {
	[Theory]
	[InlineData("x", "x", "x", "1")]
	[InlineData("x:y", "y", "x:y", "2")]
	// セクションが存在しない
	[InlineData("x:y:z", "z", "x:y:z", null)]
	public void Key_Path_Value_各プロパティの値を確認する(string key, string actualKey, string actualPath, string? actualValue) {
		// Arrange
		var root = new ConfigurationBuilder()
			.AddInMemoryCollection(new Dictionary<string, string?>{
				{ "x", "1" },
				{ "x:y", "2" },
			})
			.Build();

		// Act
		var section = root.GetSection(key);

		// Assert
		Assert.Equal(actualKey, section.Key);
		Assert.Equal(actualPath, section.Path);
		Assert.Equal(actualValue, section.Value);
	}
}
