using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableZip {
		public static IEnumerable<object[]> GetTestDataForZip() {
			yield return new object[] {
				// first
				new[] { 1, 2, 3 },
				// second
				new[] { 6, 5, 4 },
				// expected
				new [] { (1, 6), (2, 5), (3, 4) },
			};

			// 2つのシーケンスの要素数が異なる場合は、少ない方の要素数で列挙される
			// first < second
			yield return new object[] {
				new[] { 1, 2, 3 },
				new[] { 6, 5, 4, -1 },
				new [] { (1, 6), (2, 5), (3, 4) },
			};
			// first > second
			yield return new object[] {
				new[] { 1, 2, 3, -1 },
				new[] { 6, 5, 4 },
				new [] { (1, 6), (2, 5), (3, 4) },
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForZip))]
		public void Zip_2つのシーケンスからタプルを生成する(IEnumerable<int> first, IEnumerable<int> second, IEnumerable<(int, int)> expected) {
			// Arrange
			// Act
			var actual = first.Zip(second);

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
