using System.Collections;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace SampleTest.Text.Json;

// JsonSerializerのコレクション系のシリアライズ・デシリアライズのテスト
public class JsonSerializerCollectionTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	// レコード型のサンプル
	private record SampleItem(int Number, string Text);

	private class SampleWithEnumerable {
		public IEnumerable<SampleItem> Items { get; init; } = [];
	}

	private static readonly JsonSerializerOptions _options = new() {
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
	};

	[Fact]
	public void Serialize_IEnumerableを配列にシリアライズできる() {
		// Arrange

		// Act
		var sample = new SampleWithEnumerable {
			Items = [
				new SampleItem(1, "a"),
			],
		};
		var actual = JsonSerializer.Serialize(sample, _options);

		// Assert
		Assert.Equal(@"{""items"":[{""number"":1,""text"":""a""}]}", actual);
	}

	[Fact]
	public void Serialize_空のコレクションは配列としてシリアライズされる() {
		// Arrange
		var sample = new SampleWithEnumerable {
			// Itemsは空のコレクション
		};

		// Act
		var actual = JsonSerializer.Serialize(sample, _options);

		// Assert
		Assert.Equal(@"{""items"":[]}", actual);
	}

	[Fact]
	public void Serialize_空のコレクションをシリアライズしない() {
		// Arrange
		var sample = new SampleWithEnumerable {
			// Itemsは空のコレクション
		};

		// 空のコレクションを返すプロパティのJSONコントラクトを変更する
		// 参考
		// https://learn.microsoft.com/ja-jp/dotnet/standard/serialization/system-text-json/custom-contracts
		var modifier = (JsonTypeInfo typeInfo) => {
			if (typeInfo.Kind is not JsonTypeInfoKind.Object) {
				return;
			}
			var properties = typeInfo.Properties.Where(property => property.PropertyType.IsAssignableTo(typeof(IEnumerable)));
			foreach (var property in properties) {
				var getter = property.Get;
				// プリパティの値が空のコレクションであればnullを返す
				property.Get = (obj) => {
					if (getter?.Invoke(obj) is not IEnumerable values) {
						return null;
					}

					return values.Cast<object>().Any()
						? values
						: null;
				};
			}
		};

		var options = new JsonSerializerOptions {
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			// nullをシリアライズしない
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			TypeInfoResolver = new DefaultJsonTypeInfoResolver {
				Modifiers = { modifier }
			}
		};

		// Act
		var actual = JsonSerializer.Serialize(sample, options);

		// Assert
		Assert.Equal(@"{}", actual);
	}

	[Fact]
	public void Deserialize_配列をIEnumerableにデシリアライズできる() {
		// Arrange

		// Act
		var json = @"{""items"":[{""number"":1,""text"":""a""}]}";
		var actual = JsonSerializer.Deserialize<SampleWithEnumerable>(json, _options)!;

		// Assert
		var item = Assert.Single(actual.Items);
		Assert.Equal(1, item.Number);
		Assert.Equal("a", item.Text);
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
		Assert.Equal([1, null, 2], actual);
	}

	[Fact(DisplayName = "Deserialize_nullが含まれる文字列の配列をIEnumerable<string>にデシリアライズできる")]
	public void Deserialize_StringArrayWithNull_NotNullable_OK() {
		// Arrange
		var json = @"[""a"", null, ""b""]";

		// Act
		var actual = JsonSerializer.Deserialize<IEnumerable<string>>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(["a", null, "b"], actual);
	}

	[Fact(DisplayName = "Deserialize_nullが含まれる文字列の配列をIEnumerable<string?>にデシリアライズできる")]
	public void Deserialize_StringArrayWithNull_Nullable_OK() {
		// Arrange
		var json = @"[""a"", null, ""b""]";

		// Act
		var actual = JsonSerializer.Deserialize<IEnumerable<string?>>(json, _options);

		// Assert
		Assert.NotNull(actual);
		Assert.Equal(["a", null, "b"], actual);
	}
}
