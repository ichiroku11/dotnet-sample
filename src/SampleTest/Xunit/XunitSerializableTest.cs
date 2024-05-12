namespace SampleTest.Xunit;

public class XunitSerializableTest {
	// TheoryDataの型引数がシリアラズできる必要がある
	// テストデータごとにテスト結果が把握できる
	// https://xunit.net/xunit.analyzers/rules/xUnit1045
	public static TheoryData<string> GetTheoryData_Test_String() {
		return new() {
			"a",
			"b",
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Test_String))]
	public void Test_String(string value) {
		// Arrange
		// Act
		// Assert
		// 特に意味はないAssert
		Assert.NotEmpty(value);
	}

	// TheoryDataの型引数がシリアラズできないので
	// テストデータごとにテスト結果が把握できない（テスト結果が1つにまとめる）
	public class SampleNotSerializable(string value) {
		public string Value { get; } = value;
	}

	public static TheoryData<SampleNotSerializable> GetTheoryData_Test_NotSerializable() {
		return new() {
			new SampleNotSerializable("a"),
			new SampleNotSerializable("b"),
		};
	}

#pragma warning disable xUnit1045
	[Theory]
	[MemberData(nameof(GetTheoryData_Test_NotSerializable))]
	public void Test_NotSerializable(SampleNotSerializable sample) {
		// Arrange
		// Act
		// Assert
		// 特に意味はないAssert
		Assert.NotEmpty(sample.Value);
	}
#pragma warning restore xUnit1045

	// テストデータごとにテスト結果を把握するために、
	// シリアライズできないテストデータ（TheoryDataの型引数）はIXunitSerializableを実装する
	// IXunitSerializableを実装するには、パラメーターが存在しないパブリックコンストラクターが必要
	// https://xunit.net/xunit.analyzers/rules/xUnit3001
	public class SampleSerializable() : IXunitSerializable {
		public string Value { get; set; } = "";

		public void Deserialize(IXunitSerializationInfo info) {
			Value = info.GetValue<string>(nameof(Value));
		}

		public void Serialize(IXunitSerializationInfo info) {
			info.AddValue(nameof(Value), Value, typeof(string));
		}
	}

	public static TheoryData<SampleSerializable> GetTheoryData_Test_Serializable() {
		return new() {
			new SampleSerializable { Value = "a" },
			new SampleSerializable { Value = "b" },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Test_Serializable))]
	public void Test_Serializable(SampleSerializable sample) {
		// Arrange
		// Act
		// Assert
		// 特に意味はないAssert
		Assert.NotEmpty(sample.Value);
	}
}
