using System.Text.Json;

namespace SampleTest.Text.Json;

// JsonSerializerのコレクション系のシリアライズ・デシリアライズのテスト
public class JsonSerializerCollectionTest {
	// レコード型のサンプル
	private record SampleItem(int Number, string Text);

	private class SampleWithEnumerable {
		public IEnumerable<SampleItem> Items { get; init; } = Enumerable.Empty<SampleItem>();
	}

	private static readonly JsonSerializerOptions _options = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	private readonly ITestOutputHelper _output;

	public JsonSerializerCollectionTest(ITestOutputHelper output) {
		_output = output;
	}

	[Fact]
	public void Serialize_IEnumerableを配列にシリアライズできる() {
		// Arrange

		// Act
		var sample = new SampleWithEnumerable {
			Items = new[] {
					new SampleItem(1, "a"),
				},
		};
		var actual = JsonSerializer.Serialize(sample, _options);

		// Assert
		Assert.Equal(@"{""items"":[{""number"":1,""text"":""a""}]}", actual);
	}

	[Fact]
	public void Deserialize_配列をIEnumerableにデシリアライズできる() {
		// Arrange

		// Act
		var json = @"{""items"":[{""number"":1,""text"":""a""}]}";
		var actual = JsonSerializer.Deserialize<SampleWithEnumerable>(json, _options)!;

		// Assert
		Assert.Single(actual.Items);
		Assert.Collection(
			actual.Items,
			item => {
				Assert.Equal(1, item.Number);
				Assert.Equal("a", item.Text);
			});
	}

	[Fact(DisplayName = "Deserialize_nullが含まれる数値の配列をIEnumerable<int>にデシリアライズできない")]
	public void Deserialize_NumberArrayWithNull_NotNullable_Failed() {
		// Arrange
		var json = @"[1, null, 2]";

		// Act
		// Assert
		var exeption = Assert.Throws<JsonException>(() => {
			JsonSerializer.Deserialize<IEnumerable<int>>(json, _options);
		});
		_output.WriteLine(exeption.Message);
	}

	[Fact(DisplayName = "Deserialize_nullが含まれる数値の配列をIEnumerable<int?>にデシリアライズできる")]
	public void Deserialize_NumberArrayWithNull_Nullable_OK() {
		// Arrange
		var json = @"[1, null, 2]";

		// Act
		var actual = JsonSerializer.Deserialize<IEnumerable<int?>>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(new int?[] { 1, null, 2 }, actual);
	}

	[Fact(DisplayName = "Deserialize_nullが含まれる文字列の配列をIEnumerable<string>にデシリアライズできる")]
	public void Deserialize_StringArrayWithNull_NotNullable_OK() {
		// Arrange
		var json = @"[""a"", null, ""b""]";

		// Act
		var actual = JsonSerializer.Deserialize<IEnumerable<string>>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(new string?[] { "a", null, "b" }, actual);
	}
}
