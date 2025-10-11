namespace SampleTest.Collections;

public class DictionaryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Add_同じキーを追加すると例外がスローされる() {
		// Arrange
		var source = new Dictionary<string, string>() {
			{ "a", "A" },
			{ "b", "B" },
		};

		// Act
		var exception = Record.Exception(() => {
			new Dictionary<string, string>(source).Add("a", "X");
		});

		// Assert
		Assert.IsType<ArgumentException>(exception);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void Indexer_存在しないキーを使ってアクセスするとKeyNotFoundExceptionが発生する() {
		// Arrange
		var dictionary = new Dictionary<string, string>() {
		};

		// Act
		var exception = Record.Exception(() => {
			var _ = dictionary["notfound"];
		});

		// Assert
		Assert.IsType<KeyNotFoundException>(exception);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void CollectionInitializer_コレクション初期化子で同じキーを追加しても例外がスローされる() {
		// Arrange
		var source = new Dictionary<string, string>() {
			{ "a", "A" },
			{ "b", "B" },
		};

		// Act
		var exception = Record.Exception(() => {
			var _ = new Dictionary<string, string>(source) {
				{ "a", "X" }
			};
		});

		// Assert
		Assert.IsType<ArgumentException>(exception);
		_output.WriteLine(exception.Message);
	}
}
