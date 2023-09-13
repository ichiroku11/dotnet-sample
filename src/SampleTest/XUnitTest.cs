using System.Collections;

namespace SampleTest;

// xUnit.netの使い方
public class XUnitTest : IDisposable {
	// デバッグのメッセージの出力用
	private readonly ITestOutputHelper _output;

	public XUnitTest(ITestOutputHelper output) {
		_output = output;

		// 初期化処理はコンストラクタで行う
		// テストごとに呼ばれる
		_output.WriteLine("Setup");
	}

	public void Dispose() {
		// 後処理はDisposeで行う
		// テストごとに呼ばれる
		_output.WriteLine("Teardown");

		GC.SuppressFinalize(this);
	}

	// パラメータを使わないテスト
	[Fact]
	public void Fact属性でパラメータを使わないテスト() {
		Assert.True(true);
	}

	// パラメータを使ったテスト
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
