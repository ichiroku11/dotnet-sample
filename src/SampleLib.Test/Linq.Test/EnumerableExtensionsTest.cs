using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace SampleLib.Linq.Test;

public class EnumerableExtensionsTest {
	public record Sample(int Number, string Text);

	public static IEnumerable<object[]> GetTestDataForDistinctAnonymousObjectKey() {
		yield return new object[] {
				// source
				new[] {
					new Sample(1, "x"),
					new Sample(1, "x"),
				},
				// distinct
				new[] {
					new Sample(1, "x"),
				}
			};

		yield return new object[] {
				// source
				new[] {
					new Sample(1, "x"),
					new Sample(1, "y"),
				},
				// distinct
				new[] {
					new Sample(1, "x"),
					new Sample(1, "y"),
				}
			};

		yield return new object[] {
				// source
				new[] {
					new Sample(1, "x"),
					new Sample(2, "x"),
				},
				// distinct
				new[] {
					new Sample(1, "x"),
					new Sample(2, "x"),
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
