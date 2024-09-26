using System.Net;

namespace SampleTest.Net;

public class HttpListenerTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Properties_生成したインスタンスのプロパティを確認する() {
		// Arrange
		using var listener = new HttpListener();

		// Act
		// Assert
		Assert.Empty(listener.DefaultServiceNames);
		Assert.False(listener.IgnoreWriteExceptions);
		Assert.False(listener.IsListening);
		Assert.Empty(listener.Prefixes);
		Assert.Null(listener.Realm);
	}

	[Fact]
	public void GetContext_インスタンスを生成した直後に呼び出すと例外が発生する() {
		// Arrange
		using var listener = new HttpListener();

		// Act
		var exception = Record.Exception(() => listener.GetContext());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);
	}

	[Fact]
	public void GetContext_Startするだけでは例外が発生する() {
		// Arrange
		using var listener = new HttpListener();

		listener.Start();

		// Act
		var exception = Record.Exception(() => listener.GetContext());

		// Assert
		Assert.IsType<InvalidOperationException>(exception);
		_output.WriteLine(exception.Message);

		listener.Stop();
	}

	[Fact]
	public void IsListening_StartするとtrueになりStopするとfalseになる() {
		// Arrange
		using var listener = new HttpListener();

		// Act
		listener.Start();
		var actualStarted = listener.IsListening;

		listener.Stop();
		var actualStopped = listener.IsListening;

		// Assert
		Assert.True(actualStarted);
		Assert.False(actualStopped);
	}
}
