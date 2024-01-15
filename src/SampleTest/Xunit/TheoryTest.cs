namespace SampleTest.Xunit;

public class TheoryTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Theory]
	// テストデータを埋め込む
	[InlineData(2)]
	[InlineData(4)]
	public void Theory属性とInlineData属性でパラメータを使ったテスト(int value) {
		Assert.Equal(0, value % 2);
	}


	// テストデータを生成するプロパティ
	public static TheoryData<int> EvenNumbers => new() { 2, 4 };

	[Theory]
	// プロパティからテストデータを取得する
	[MemberData(nameof(EvenNumbers))]
	public void Theory属性とMemberData属性でプロパティからテストデータを生成したテスト(int value) {
		_output.WriteLine($"{value}");

		Assert.Equal(0, value % 2);
	}


	// テストデータを生成するメソッド
	public static TheoryData<int> GetEvenNumbers(int count)
		=> TheoryDataFactory.CreateFrom(Enumerable.Range(0, count).Select(value => value * 2));

	[Theory]
	// メソッドからテストデータを取得する
	[MemberData(nameof(GetEvenNumbers), 2)]
	public void Theory属性とMemberData属性でメソッドからテストデータを生成したテスト(int value) {
		_output.WriteLine($"{value}");

		Assert.Equal(0, value % 2);
	}


	// テストデータを生成するクラス
	public class EvenNumberEnumerable : TheoryData<int> {
		public EvenNumberEnumerable() {
			Add(2);
			Add(4);
		}
	}

	[Theory]
	// クラスからテストデータを取得する
	[ClassData(typeof(EvenNumberEnumerable))]
	public void Theory属性とClassData属性でクラスからテストデータを生成したテスト(int value) {
		_output.WriteLine($"{value}");

		Assert.Equal(0, value % 2);
	}
}
