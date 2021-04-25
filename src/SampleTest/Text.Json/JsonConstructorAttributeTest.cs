using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
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
			var sample = JsonSerializer.Deserialize<Sample1>(@"{""value1"":1, ""value2"":1}", options);

			// Assert
			Assert.Equal(0, sample.Value1);
			Assert.Equal(1, sample.Value2);
		}

		// コンストラクタが1つの場合
		private class Sample2 {
			public int Value1 { get; }
			public int Value2 { get; }

			public Sample2(int value1 = -1) {
				Value1 = value1;
			}
		}

		[Theory]
		[InlineData(@"{}", -1, 0)]
		[InlineData(@"{""value1"":1, ""value2"":1}", 1, 0)]
	
		public void Deserialize_コンストラクタが呼び出される(string json, int expectedValue1, int expectedValue2) {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};

			// Act
			var sample = JsonSerializer.Deserialize<Sample2>(json, options);

			// Assert
			Assert.Equal(expectedValue1, sample.Value1);
			Assert.Equal(expectedValue2, sample.Value2);
		}

		// 引数があるコンストラクタがある場合
		private class Sample3 {
			public int Value1 { get; }
			public int Value2 { get; }

			public Sample3(int value1, int value2) {
				Value1 = value1;
				Value2 = value2;
			}
		}

		[Theory]
		[InlineData(@"{}", 0, 0)]
		[InlineData(@"{""value1"":1, ""value2"":2}", 1, 2)]

		public void Deserialize_引数があるコンストラクタを呼び出せる(string json, int expectedValue1, int expectedValue2) {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNameCaseInsensitive = true,
			};

			// Act
			var sample = JsonSerializer.Deserialize<Sample3>(json, options);

			// Assert
			Assert.Equal(expectedValue1, sample.Value1);
			Assert.Equal(expectedValue2, sample.Value2);
		}

		// コンストラクタが複数ある場合
		private class Sample4 {
			public int Value { get; }

			public Sample4() {
			}

			public Sample4(int value) {
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
			var sample = JsonSerializer.Deserialize<Sample4>(json, options);

			// Assert
			Assert.Equal(expectedValue, sample.Value);
		}
	}
}
