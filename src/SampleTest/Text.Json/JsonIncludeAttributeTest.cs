using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class JsonIncludeAttributeTest {
		// private getterはシリアライズされない
		// private getterにJsonIncludeAttributeを指定するのシリアライズされる
		private class Sample1 {
			public int Value1 { private get; set; }

			[JsonInclude]
			public int Value2 { private get; set; }
		}

		[Fact]
		public void Serialize_JsonIncludeAttributeを使ってprivateなgetterをシリアライズする() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

			// Act
			var actual = JsonSerializer.Serialize(new Sample1 { Value1 = 1, Value2 = 2 }, options);

			// Assert
			Assert.Equal(@"{""value2"":2}", actual);
		}
	}
}
