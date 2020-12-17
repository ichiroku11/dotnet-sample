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
		public static IEnumerable<object[]> GetTestDataForParseQuery {
			get {
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
			}
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
