using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	// https://docs.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json-immutability
	public class JsonConstructorAttributeTest {
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
	}
}
