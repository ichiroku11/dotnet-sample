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

	// TheoryDataの型引数がシリアラズできないので
	// テストデータごとにテスト結果が把握できない（テスト結果が1つにまとめる）
	public class Sample(string value) {
		public string Value { get; } = value;
	}

	public static TheoryData<Sample> GetTheoryData_Test_NotSerializable() {
		return new() {
			new Sample("a"),
			new Sample("b"),
		};
	}

#pragma warning disable xUnit1045
	[Theory]
	[MemberData(nameof(GetTheoryData_Test_NotSerializable))]
	public void Test_NotSerializable(Sample sample) {
		// Arrange
		// Act
		// Assert
		// 特に意味はないAssert
		Assert.NotEmpty(sample.Value);
	}
#pragma warning restore xUnit1045
}
