namespace SampleTest;

public class NullableTest(ITestOutputHelper output) {
	// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/builtin-types/nullable-value-types#how-to-identify-a-nullable-value-type

	private readonly ITestOutputHelper _output = output;

	// Nullable.GetUnderlyingTypeを使うと、指定した型ががnullableか判定できる
	[Theory]
	[InlineData(typeof(int?), "Int32")]
	[InlineData(typeof(long?), "Int64")]
	[InlineData(typeof(DateTime?), "DateTime")]
	public void GetUnderlyingType_引数にnull許容型の型を指定すると型引数の型を取得できる(Type type, string expectedName) {
		// Arrange
		// Act
		// Returns the underlying type argument of the specified nullable type.
		var actual = Nullable.GetUnderlyingType(type);
		_output.WriteLine(actual?.Name);

		// Assert
		Assert.Equal(expectedName, actual?.Name);
	}

	[Theory]
	[InlineData(typeof(int))]
	[InlineData(typeof(long))]
	[InlineData(typeof(DateTime))]
	// 参照型もnullになる様子
	[InlineData(typeof(string))]
	[InlineData(typeof(object))]
	public void GetUnderlyingType_引数にnull許容型ではない型を指定するとnullを返す(Type type) {
		// Arrange
		// Act
		var actual = Nullable.GetUnderlyingType(type);

		// Assert
		Assert.Null(actual);
	}

	[Fact]
	public void GetValueOrDefault_値がnullではない場合はその値を取得できる() {
		// Arrange
		int? value = 1;

		// Act
		var actual = value.GetValueOrDefault();

		// Assert
		Assert.Equal(1, actual);
	}

	[Fact]
	public void GetValueOrDefault_値がnullの場合はその型のデフォルト値を取得できる() {
		// Arrange
		int? value = null;

		// Act
		var actual = value.GetValueOrDefault();

		// Assert
		Assert.Equal(0, actual);
	}

	[Fact]
	public void GetValueOrDefault_値がnullの場合は指定した値を取得できる() {
		// Arrange
		int? value = null;

		// Act
		var actual = value.GetValueOrDefault(-1);

		// Assert
		Assert.Equal(-1, actual);
	}

	[Theory]
	// 1つ目の引数がnullの場合
	[InlineData(null, 1)]
	// どちらの引数もnullではなく、1つ目の引数が小さい場合
	[InlineData(-1, 1)]
	public void Comapre_結果が0より小さい(int? value1, int? value2) {
		// Arrange
		// Act
		var result = Nullable.Compare(value1, value2);

		// Assert
		Assert.True(result < 0);
	}

	[Theory]
	// どちらの引数もnullの場合
	[InlineData(null, null)]
	// どちらの引数もnullではなく、非JK異数が等しい場合
	[InlineData(1, 1)]
	public void Comapre_結果が0と等しい(int? value1, int? value2) {
		// Arrange
		// Act
		var result = Nullable.Compare(value1, value2);

		// Assert
		Assert.True(result == 0);
	}

	[Theory]
	// 2つ目の引数がnullの場合
	[InlineData(1, null)]
	// どちらの引数もnullではなく、1つ目の引数が大きい場合
	[InlineData(1, -1)]
	public void Comapre_結果が0より大きい(int? value1, int? value2) {
		// Arrange
		// Act
		var result = Nullable.Compare(value1, value2);

		// Assert
		Assert.True(result > 0);
	}
}
