using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Text.Json {
	public class JsonStringEnumConverterTest {
		// enum<=>文字列・数値の変換を試すテスト
		private enum SampleCode {
			Unknown = 0,
			Ok,
			Error,
		}

		private class Sample {
			public SampleCode Code { get; set; }
		}

		[Fact]
		public void デフォルトではenumは数値にシリアライズされる() {
			// Arrange
			var options = new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};
			var data = new Sample {
				Code = SampleCode.Ok,
			};

			// Act
			var json = JsonSerializer.Serialize(data, options);

			// Assert
			Assert.Equal(@"{""code"":1}", json);
		}


	}
}
