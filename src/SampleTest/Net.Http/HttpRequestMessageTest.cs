using System.Net;

namespace SampleTest.Net.Http;

public class HttpRequestMessageTest {
	[Fact]
	public void Properties_デフォルトコンストラクターで生成したインスタンスのプロパティを確認する() {
		// Arrange
		// Act
		var request = new HttpRequestMessage();

		// Assert
		Assert.Equal(HttpMethod.Get, request.Method);
		Assert.Null(request.RequestUri);
		Assert.Same(HttpVersion.Version11, request.Version);
		Assert.Equal(HttpVersionPolicy.RequestVersionOrLower, request.VersionPolicy);
	}
}
