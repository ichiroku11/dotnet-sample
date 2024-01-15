namespace SampleTest;

public class ExceptionTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Message_コンストラクタで指定したメッセージを取得できる() {
		var exception = new Exception("outer", new Exception("inner"));

		_output.WriteLine(exception.ToString());

		Assert.Equal("outer", exception.Message);
		Assert.Equal("inner", exception.InnerException?.Message);
	}
}
