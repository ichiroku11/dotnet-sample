using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Xunit;

namespace SampleTest.AspNetCore;

public class QueryHelpersTest {
	public static TheoryData<string, IEnumerable<KeyValuePair<string, string>>, string> GetTheoryDataForAddQueryString() {
		return new() {
			{
				"https://example.jp",
				new[] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"https://example.jp?a=1&b=2"
			},
			// URLは相対URLでも問題なく追加できる
			{
				"~/test",
				new[] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"~/test?a=1&b=2"
			},
			// URLは空文字でも問題なく追加できる
			{
				"",
				new[] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"?a=1&b=2"
			},
			// URLに同じクエリ文字列が含まれていても追加される
			{
				"https://example.jp?a=1",
				new[] {
					new KeyValuePair<string, string>("a", "1"),
					new KeyValuePair<string, string>("b", "2"),
				},
				"https://example.jp?a=1&a=1&b=2"
			}
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForAddQueryString))]
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

	public static TheoryData<string, Dictionary<string, StringValues>> GetTheoryDataForParseQuery() {
		return new() {
			// 先頭が「?」で始まる
			{
				"?a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				}
			},
			// 先頭が「?」で始まらない
			{
				"a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				}
			},
			// クエリ文字列がない
			{ "", new Dictionary<string, StringValues> { } },
			{ "?", new Dictionary<string, StringValues> { } },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForParseQuery))]
	public void ParseQuery_パースできる(string query, Dictionary<string, StringValues> expected) {
		// Arrange
		// Act
		var actual = QueryHelpers.ParseQuery(query);

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<string, Dictionary<string, StringValues>?> GetTheoryDataForParseNullableQuery() {
		return new() {
			// 先頭が「?」で始まる
			{
				"?a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				}
			},
			// 先頭が「?」で始まらない
			{
				"a=1&b=2",
				new Dictionary<string, StringValues> {
					["a"] = "1",
					["b"] = "2",
				}
			},
			// クエリ文字列がない
			// ParseQueryの戻り値は空のDictionaryに対して、
			// ParseNullableQueryの戻り値はnullになる
			{ "", null },
			{ "?", null },
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryDataForParseNullableQuery))]
	public void ParseNullableQuery_パースできる(string query, Dictionary<string, StringValues>? expected) {
		// Arrange
		// Act
		var actual = QueryHelpers.ParseNullableQuery(query);

		// Assert
		Assert.Equal(expected, actual);
	}
}
