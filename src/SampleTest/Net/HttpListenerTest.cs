using System.Net;

namespace SampleTest.Net;

public class HttpListenerTest {
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
}
