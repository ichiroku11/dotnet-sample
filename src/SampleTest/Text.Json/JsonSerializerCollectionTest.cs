using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Text.Json;

// JsonSerializerのコレクション系のシリアライズ・デシリアライズのテスト
public class JsonSerializerCollectionTest {
	// レコード型のサンプル
	private record SampleItem(int Number, string Text);

	private class SampleWithEnumerable {
		public IEnumerable<SampleItem> Items { get; init; }
	}

	[Fact]
	public void Serialize_IEnumerableを配列にシリアライズできる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var sample = new SampleWithEnumerable {
			Items = new[] {
					new SampleItem(1, "a"),
				},
		};
		var actual = JsonSerializer.Serialize(sample, options);

		// Assert
		Assert.Equal(@"{""items"":[{""number"":1,""text"":""a""}]}", actual);
	}

	[Fact]
	public void Deserialize_配列をIEnumerableにデシリアライズできる() {
		// Arrange
		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		// Act
		var json = @"{""items"":[{""number"":1,""text"":""a""}]}";
		var actual = JsonSerializer.Deserialize<SampleWithEnumerable>(json, options);

		// Assert
		Assert.Single(actual.Items);
		Assert.Collection(
			actual.Items,
			item => {
				Assert.Equal(1, item.Number);
				Assert.Equal("a", item.Text);
			});
	}
}
