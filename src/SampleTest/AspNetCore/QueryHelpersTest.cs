using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.AspNetCore {
	public class QueryHelpersTest {
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
	}
}
