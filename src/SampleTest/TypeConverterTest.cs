using System.Collections;
using System.ComponentModel;

namespace SampleTest;

// TypeConverter クラス (System.ComponentModel) | Microsoft Docs
// https://docs.microsoft.com/ja-jp/dotnet/api/system.componentmodel.typeconverter?view=netcore-3.1
public class TypeConverterTest {
	public enum Fruit {
		None = 0,
		Apple,
		Banana,
	}

	private static readonly IEnumerable<Fruit> _fruits = Enum.GetValues(typeof(Fruit)).Cast<Fruit>();

	// Fruit一覧を取得
	public static TheoryData<Fruit> GetTheoryDataFruits()
		=> TheoryDataFactory.CreateFrom(_fruits);

	// Fruit.Appleに変換できる文字列を取得
	public static TheoryData<string> GetTheoryDataStringsCanConvertToApple()
		=> TheoryDataFactory.CreateFrom(new[] { "Apple", "apple", "1" });

	[Fact]
	public void GetConverter_EnumのTypeConverterはEnumConverter() {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		// Assert
		Assert.IsType<EnumConverter>(converter);
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataFruits))]
	public void IsValid_enum値が定義されていることを判定できる(Fruit fruit) {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		// Assert
		Assert.True(converter.IsValid(fruit));
	}

	[Fact]
	public void IsValid_enum値が定義されている数値はtrueになる() {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		// Assert
		Assert.True(converter.IsValid(1));
	}

	[Theory]
	[InlineData((int)Fruit.None - 1)]
	[InlineData((int)Fruit.Banana + 1)]
	public void IsValid_enum値が定義されていない数値はfalseになる(int nofruit) {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		// Assert
		Assert.False(converter.IsValid(nofruit));
	}

	[Fact]
	public void ConvertToString_Enumを文字列に変換できる() {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		var actual = converter.ConvertToString(Fruit.Apple);

		// Assert
		Assert.Equal("Apple", actual, StringComparer.OrdinalIgnoreCase);
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataStringsCanConvertToApple))]
	public void ConvertFromString_文字列からEnumに変換できる(string value) {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		var actual = converter.ConvertFromString(value) is Fruit fruit
			? fruit
			: throw new InvalidOperationException();

		// Assert
		Assert.Equal(Fruit.Apple, actual);
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataStringsCanConvertToApple))]
	public void ConvertFrom_文字列からEnumに変換できる(string value) {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		var actual = converter.ConvertFrom(value) is Fruit fruit
			? fruit
			: throw new InvalidOperationException();

		// Assert
		Assert.Equal(Fruit.Apple, actual);
	}

	[Fact]
	public void ConvertFrom_数値からEnumに変換できない() {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		// Assert
		Assert.False(converter.CanConvertFrom(typeof(int)));

		// 例外が発生する
		Assert.Throws<NotSupportedException>(() => {
			converter.ConvertFrom(1);
		});
	}

	[Fact]
	public void GetStandardValues_enum値のコレクションを取得できる() {
		// Arrange
		var converter = TypeDescriptor.GetConverter(typeof(Fruit));

		// Act
		var actual = converter.GetStandardValues() is ICollection values
			? values.Cast<Fruit>()
			: throw new InvalidOperationException();

		// Assert
		Assert.Equal(_fruits, actual);
	}
}
