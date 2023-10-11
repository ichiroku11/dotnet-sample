using System.Text.Json.Serialization;
using System.Text.Json;

namespace SampleTest.Text.Json;

public class JsonDerivedTypeAttributeTest {
	private static readonly JsonSerializerOptions _options
		= new() {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

	// JsonDerivedTypeAttributeを使わず
	// 派生クラスを基本クラスとしてシリアライズする
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

	// JsonDerivedTypeAttributeを使って
	// 派生クラスを基本クラスとしてシリアライズする
	[JsonDerivedType(typeof(DerivedWithAttribute))]
	private abstract class BaseWithAttribute {
		// 属性でJSON文字列のプロパティの並び順を制御する
		[JsonPropertyOrder(1)]
		public int Base { get; init; }
	}

	private class DerivedWithAttribute : BaseWithAttribute {
		[JsonPropertyOrder(2)]
		public int Derived { get; init; }
	}

	[Fact]
	public void Serialize_基本クラスの型を指定してシリアライズしても派生クラスの情報が欠落しない() {
		// Arrange
		var derived = new DerivedWithAttribute {
			Base = 1,
			Derived = 2,
		};

		// Act
		var actual = JsonSerializer.Serialize<BaseWithAttribute>(derived, _options);

		// Assert
		// 派生クラスの情報が欠落しない
		Assert.Equal(@"{""base"":1,""derived"":2}", actual);
	}

	[Fact]
	public void Serialize_基本クラスとしてシリアライズしても派生クラスの情報が欠落しない() {
		// Arrange
		BaseWithAttribute derived = new DerivedWithAttribute {
			Base = 1,
			Derived = 2,
		};

		// Act
		var actual = JsonSerializer.Serialize(derived, _options);

		// Assert
		// 派生クラスの情報が欠落しない
		Assert.Equal(@"{""base"":1,""derived"":2}", actual);
	}


	// 型の判別子を使ってシリアライズ・デシリアライズする
	[JsonDerivedType(typeof(DerivedWithAttributeAndDiscriminator), "derived")]
	private abstract class BaseWithAttributeAndDiscriminator {
		// 属性でJSON文字列のプロパティの並び順を制御する
		[JsonPropertyOrder(1)]
		public int Base { get; init; }
	}

	private class DerivedWithAttributeAndDiscriminator : BaseWithAttributeAndDiscriminator {
		[JsonPropertyOrder(2)]
		public int Derived { get; init; }
	}

	[Fact]
	public void Serialize_型の判別子がJSONに出力される() {
		// Arrange
		BaseWithAttributeAndDiscriminator derived = new DerivedWithAttributeAndDiscriminator {
			Base = 1,
			Derived = 2,
		};

		// Act
		var actual = JsonSerializer.Serialize(derived, _options);

		// Assert
		// $typeプロパティは必ず先頭なのか？
		Assert.Equal(@"{""$type"":""derived"",""base"":1,""derived"":2}", actual);
	}

	[Fact]
	public void Deserialize_型の判別子を使ってデシリアライズする() {
		// Arrange
		var json = @"{""$type"":""derived"",""base"":1,""derived"":2}";

		// Act
		// 基本クラスとしてデシリアライズする
		var actualBase = JsonSerializer.Deserialize<BaseWithAttributeAndDiscriminator>(json, _options);

		// Assert
		// 型は派生クラスになる
		var actualDerived = Assert.IsType<DerivedWithAttributeAndDiscriminator>(actualBase);
		Assert.Equal(1, actualDerived.Base);
		Assert.Equal(2, actualDerived.Derived);
	}
}
