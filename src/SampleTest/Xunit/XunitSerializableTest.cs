namespace SampleTest.Xunit;

public class XunitSerializableTest {
	// TheoryDataの型引数がシリアラズできる必要がある
	// テストデータごとにテスト結果が把握できる
	// https://xunit.net/xunit.analyzers/rules/xUnit1045
	public static TheoryData<string> GetTheoryData_Test_Serializable() {
		return new() {
			"a",
			"b",
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Test_Serializable))]
	public void Test_Serializable(string value) {
		// Arrange
		// Act
		// Assert
		// 特に意味はないAssert
		Assert.NotEmpty(value);
	}
}
