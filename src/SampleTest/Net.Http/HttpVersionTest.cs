using System.Net;

namespace SampleTest.Net.Http;

public class HttpVersionTest {
	[Fact]
	public void Fields_各バージョンを確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(new Version(1, 1), HttpVersion.Version11);
		Assert.Equal(new Version(2, 0), HttpVersion.Version20);
		Assert.Equal(new Version(3, 0), HttpVersion.Version30);
	}
}
