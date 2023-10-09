using System.Text.Json.Serialization;
using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonDerivedTypeAttributeTest {
	private static readonly JsonSerializerOptions _options
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	private abstract class BaseNoAttribute {
		// 属性でJSON文字列のプロパティの並び順を制御する
		[JsonPropertyOrder(1)]
		public int Base { get; init; }
	}

	private class DerivedNoAttribute : BaseNoAttribute {
		[JsonPropertyOrder(2)]
		public int Derived { get; init; }
	}

	[Fact]
	public void Serialize_基本クラスの型を指定してシリアライズすると派生クラスの情報が欠落する() {
		// Arrange
		var derived = new DerivedNoAttribute {
			Base = 1,
			Derived = 2,
		};

		// Act
		var actual = JsonSerializer.Serialize<BaseNoAttribute>(derived, _options);

		// Assert
		// 派生クラスの情報が欠落する
		Assert.Equal(@"{""base"":1}", actual);
	}

	[Fact]
	public void Serialize_基本クラスとしてシリアライズすると派生クラスの情報が欠落する() {
		// Arrange
		BaseNoAttribute derived = new DerivedNoAttribute {
			Base = 1,
			Derived = 2,
		};

		// Act
		var actual = JsonSerializer.Serialize(derived, _options);

		// Assert
		// 派生クラスの情報が欠落する
		Assert.Equal(@"{""base"":1}", actual);
	}
}
