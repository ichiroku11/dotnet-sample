using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace SampleTest.AspNetCore;

public class QueryHelpersTest {
	public static IEnumerable<object[]> GetTestDataForAddQueryString() {
		yield return new object[] {
				"https://example.jp",
				new [] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"https://example.jp?a=1&b=2"
			};

		// URLは相対URLでも問題なく追加できる
		yield return new object[] {
				"~/test",
				new [] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"~/test?a=1&b=2"
			};

		// URLは空文字でも問題なく追加できる
		yield return new object[] {
				"",
				new [] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"?a=1&b=2"
			};

		// URLに同じクエリ文字列が含まれていても追加される
		yield return new object[] {
				"https://example.jp?a=1",
				new [] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"https://example.jp?a=1&a=1&b=2"
			};
	}

	[Theory]
	[MemberData(nameof(GetTestDataForAddQueryString))]
	public void AddQueryString_クエリ文字列を追加できる(string url, IEnumerable<KeyValuePair<string, string?>> query, string expected) {
		// Arrange
		// Act
		var actual = QueryHelpers.AddQueryString(url, query);

		// Assert
		Assert.Equal(expected, actual);
	}

	[Fact]
	public void AddQueryString_クエリ文字列はURLエンコードされる() {
		// Arrange
		// Act
		var actual = QueryHelpers.AddQueryString("https://example.jp/login", "returnUrl", "https://example.jp");

		// Assert
		Assert.Equal("https://example.jp/login?returnUrl=https%3A%2F%2Fexample.jp", actual);
	}

	public static IEnumerable<object[]> GetTestDataForParseQuery() {
		// 先頭が「?」で始まる
		yield return new object[] {
				"?a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				},
			};

		// 先頭が「?」で始まらない
		yield return new object[] {
				"a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				},
			};

		// クエリ文字列がない
		yield return new object[] { "", new Dictionary<string, StringValues> { }, };
		yield return new object[] { "?", new Dictionary<string, StringValues> { }, };
	}

	[Theory]
	[MemberData(nameof(GetTestDataForParseQuery))]
	public void ParseQuery_パースできる(string query, Dictionary<string, StringValues> expected) {
		// Arrange
		// Act
		var actual = QueryHelpers.ParseQuery(query);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static IEnumerable<object?[]> GetTestDataForParseNullableQuery() {
		// 先頭が「?」で始まる
		yield return new object[] {
				"?a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				},
			};

		// 先頭が「?」で始まらない
		yield return new object[] {
				"a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				},
			};

		// クエリ文字列がない
		// ParseQueryの戻り値は空のDictionaryに対して、
		// ParseNullableQueryの戻り値はnullになる
		yield return new object?[] { "", null, };
		yield return new object?[] { "?", null, };
	}

	[Theory]
	[MemberData(nameof(GetTestDataForParseNullableQuery))]
	public void ParseNullableQuery_パースできる(string query, Dictionary<string, StringValues>? expected) {
		// Arrange
		// Act
		var actual = QueryHelpers.ParseNullableQuery(query);

		// Assert
		Assert.Equal(expected, actual);
	}
}
