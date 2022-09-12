namespace SampleTest;

public class UriTest {
	[Theory]
	[InlineData("http://example.jp", "", "http://example.jp/")]
	[InlineData("http://example.jp", "/", "http://example.jp/")]
	[InlineData("http://example.jp", "app/home", "http://example.jp/app/home")]
	[InlineData("http://example.jp", "/app/home", "http://example.jp/app/home")]
	[InlineData("http://example.jp/", "", "http://example.jp/")]
	[InlineData("http://example.jp/", "/", "http://example.jp/")]
	[InlineData("http://example.jp/", "app/home", "http://example.jp/app/home")]
	[InlineData("http://example.jp/", "/app/home", "http://example.jp/app/home")]
	// 「/」で始まる相対パスはドメイン名からの相対パスになる
	// ベースURLがパスを含む場合は、「/」で終わっているかどうかで生成されるURLが変わってくるので注意
	[InlineData("http://example.jp/app", "", "http://example.jp/app")]
	[InlineData("http://example.jp/app", "/", "http://example.jp/")]
	[InlineData("http://example.jp/app", "home", "http://example.jp/home")]
	[InlineData("http://example.jp/app", "/home", "http://example.jp/home")]
	[InlineData("http://example.jp/app/", "", "http://example.jp/app/")]
	[InlineData("http://example.jp/app/", "/", "http://example.jp/")]
	[InlineData("http://example.jp/app/", "home", "http://example.jp/app/home")]
	[InlineData("http://example.jp/app/", "/home", "http://example.jp/home")]
	public void Constructor_ベースURIと相対URI文字列で生成できる(string baseUri, string relativeUri, string expectedUri) {
		// Arrange
		// Act
		var actualUri = new Uri(new Uri(baseUri), relativeUri);

		// Assert
		Assert.Equal(expectedUri, actualUri.AbsoluteUri);
	}

	[Theory]
	[InlineData("http://example.jp", "http")]
	[InlineData("https://example.jp", "https")]
	public void Scheme_スキーマを取得できる(string url, string scheme) {
		// Arrange
		var uri = new Uri(url);

		// Act
		// Assert
		Assert.Equal(scheme, uri.Scheme);
	}

	[Fact]
	public void SchemeDelimiter_確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal("://", Uri.SchemeDelimiter);
	}

	[Theory]
	[InlineData("https://example.jp")]
	[InlineData("https://example.jp/")]
	[InlineData("https://example.jp/app")]
	[InlineData("file://example.jp/app")]
	public void TryCreate_絶対URLの成功する(string url) {
		// Arrange
		// Act
		var result = Uri.TryCreate(url, UriKind.Absolute, out var _);

		// Assert
		Assert.True(result);
	}

	[Theory]
	[InlineData("xyz")]
	public void TryCreate_絶対URLの作成に失敗する(string url) {
		// Arrange
		// Act
		var result = Uri.TryCreate(url, UriKind.Absolute, out var _);

		// Assert
		Assert.False(result);
	}

	[Theory]
	[InlineData("https://example.jp/path1/path2", new[] { "/", "path1/", "path2" })]
	[InlineData("https://example.jp/path1/path2/", new[] { "/", "path1/", "path2/" })]
	public void Segments_パス部分を配列として取得できる(string url, string[] segments) {
		// Arrange
		var uri = new Uri(url);

		// Act
		// Assert
		Assert.Equal(segments, uri.Segments);
	}

	[Theory]
	[InlineData("https://example.jp#fragment", "#fragment")]
	public void Fragment_フラグメントを取得できる(string url, string fragment) {
		// Arrange
		var uri = new Uri(url);

		// Act
		// Assert
		Assert.Equal(fragment, uri.Fragment);
	}

	[Theory]
	[InlineData("https://example.jp?a=1&b=2", "?a=1&b=2")]
	[InlineData("https://example.jp", "")]
	[InlineData("https://example.jp?", "?")]
	public void Query_クエリ文字列を取得できる(string url, string query) {
		// Arrange
		var uri = new Uri(url);

		// Act
		// Assert
		Assert.Equal(query, uri.Query);
	}

	[Theory]
	[InlineData("", UriHostNameType.Unknown)]
	[InlineData(default(string), UriHostNameType.Unknown)]
	[InlineData("localhost", UriHostNameType.Dns)]
	[InlineData("www.example.jp", UriHostNameType.Dns)]
	public void CheckHostName_取得できるUriHostNameTypeを確認する(string name, UriHostNameType expected) {
		// Arrange
		// Act
		var actual = Uri.CheckHostName(name);

		// Assert
		Assert.Equal(expected, actual);
	}
}
