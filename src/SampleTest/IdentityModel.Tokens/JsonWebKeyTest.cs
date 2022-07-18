using Microsoft.IdentityModel.Tokens;

namespace SampleTest.IdentityModel.Tokens;

public class JsonWebKeyTest {
	public static TheoryData<JsonWebKey> GetTheoryDataForKeyIdIsNull() {
		return new() {
			{ new JsonWebKey() },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForKeyIdIsNull))]
	public void KeyId_指定しない場合デフォルトではnull(JsonWebKey key) {
		// Arrange
		// Act
		// Assert
		Assert.Null(key.KeyId);
	}
}
