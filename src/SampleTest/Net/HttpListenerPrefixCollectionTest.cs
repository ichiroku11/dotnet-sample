using System.Net;

namespace SampleTest.Net;

public class HttpListenerPrefixCollectionTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Add_リスナーのPrefixesに追加するURLはスラッシュで終わらないと例外が発生する() {
		// Arrange
		using var listener = new HttpListener();

		// Act
		var exception = Record.Exception(() => listener.Prefixes.Add("http://localhost"));

		// Assert
		Assert.IsType<ArgumentException>(exception);
		_output.WriteLine(exception.Message);
	}
}
