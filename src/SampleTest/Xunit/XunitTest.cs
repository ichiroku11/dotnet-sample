namespace SampleTest;

public class XunitTest : IDisposable {
	// デバッグのメッセージの出力用
	private readonly ITestOutputHelper _output;

	public XunitTest(ITestOutputHelper output) {
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

	[Fact]
	public void Fact属性でパラメータを使わないテスト() {
		Assert.True(true);
	}
}
