using System.Text.Json;
using System.Text.Json.Serialization;

namespace SampleTest.Text.Json;

// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-immutability
public class JsonConstructorAttributeTest {
	// コンストラクタがない場合
	private class Sample1 {
		public int Value1 { get; }
		public int Value2 { get; init; }
	}

	[Fact]
	public void Deserialize_getterプロパティはデシリアライズされない() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<Sample1>(@"{""value1"":1, ""value2"":1}", options)!;

		// Assert
		Assert.Equal(0, sample.Value1);
		Assert.Equal(1, sample.Value2);
	}

	// コンストラクタが1つの場合
	private class Sample2 {
		public int Value { get; }

		public Sample2(int value) {
			Value = value;
		}
	}

	[Theory]
	[InlineData(@"{}", 0)]
	[InlineData(@"{""value"":1}", 1)]

	public void Deserialize_コンストラクタが呼び出される(string json, int expectedValue) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<Sample2>(json, options)!;

		// Assert
		Assert.Equal(expectedValue, sample.Value);
	}

	// コンストラクタが複数ある場合
	private class Sample3 {
		public int Value { get; }

		public Sample3() {
		}

		public Sample3(int value) {
			Value = value;
		}
	}

	[Theory]
	[InlineData(@"{}", 0)]
	[InlineData(@"{""value"":1}", 0)]

	public void Deserialize_パラメータなしのコンストラクタが呼び出される(string json, int expectedValue) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<Sample3>(json, options)!;

		// Assert
		Assert.Equal(expectedValue, sample.Value);
	}

	// JsonConstructorAttributeを使ってコンストラクタを指定する
	private class Sample4 {
		public int Value { get; }

		public Sample4() {
		}

		[JsonConstructor]
		public Sample4(int value) {
			Value = value;
		}
	}

	[Theory]
	[InlineData(@"{}", 0)]
	[InlineData(@"{""value"":1}", 1)]

	public void Deserialize_JsonConstructorAttributeで呼び出すコンストラクタを指定する(string json, int expectedValue) {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNameCaseInsensitive = true,
		};

		// Act
		var sample = JsonSerializer.Deserialize<Sample4>(json, options)!;

		// Assert
		Assert.Equal(expectedValue, sample.Value);
	}
}
