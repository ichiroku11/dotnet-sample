using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableExtensionsTest {
		public class Sample {
			public int Number { get; set; }
			public string Text { get; set; }
		}

		public static IEnumerable<object[]> GetTestDataForDistinctAnonymousObjectKey() {
			yield return new object[] {
				// source
				new[] {
					new Sample { Number = 1, Text = "x", },
					new Sample { Number = 1, Text = "x", },
				},
				// distinct
				new[] {
					new Sample { Number = 1, Text = "x", },
				}
			};

			yield return new object[] {
				// source
				new[] {
					new Sample { Number = 1, Text = "x", },
					new Sample { Number = 1, Text = "y", },
				},
				// distinct
				new[] {
					new Sample { Number = 1, Text = "x", },
					new Sample { Number = 1, Text = "y", },
				}
			};

			yield return new object[] {
				// source
				new[] {
					new Sample { Number = 1, Text = "x", },
					new Sample { Number = 2, Text = "x", },
				},
				// distinct
				new[] {
					new Sample { Number = 1, Text = "x", },
					new Sample { Number = 2, Text = "x", },
				}
			};
		}

		private static bool Equal(Sample x, Sample y)
			=> x.Number == y.Number && string.Equals(x.Text, x.Text, StringComparison.Ordinal);

		[Theory(DisplayName = "Distinct_匿名オブジェクトのキーを使ってみる")]
		[MemberData(nameof(GetTestDataForDistinctAnonymousObjectKey))]
		public void DistinctAnonymousObjectKey(IEnumerable<Sample> source, IEnumerable<Sample> expected) {
			// Arrange
			// Act
			var actual = source.Distinct(item => new { item.Number, item.Text });

			// Assert
			Assert.Equal(expected.Count(), actual.Count());
			Assert.All(actual,
				actualItem => Assert.Contains(expected, expectedItem => Equal(actualItem, expectedItem)));
		}
	}
}
