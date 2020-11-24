using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class ValueObjectTest {
		private class SampleObject : ValueObject {
			public SampleObject(string text, int nuber) {
				Text = text;
				Number = nuber;
			}

			public string Text { get; }
			public int Number { get; }

			protected override IEnumerable<object> GetAtomicValues() {
				yield return Text;
				yield return Number;
			}
		}

		[Fact]
		public void Equal_同じ値を持つ異なるValueObjectは等しい() {
			// Arrange
			// Act
			var actual = new SampleObject("Aaa", 100);
			var expected = new SampleObject("Aaa", 100);

			// Assert
			// 異なるインスタンスだけど等しい
			Assert.NotSame(expected, actual);
			Assert.Equal(expected, actual);
		}

		[Fact]
		public void NotEqual_異なる値を持つ異なるValueObjectは等しくない() {
			// Arrange
			// Act
			var actual = new SampleObject("Aaa", 100);
			// Numberが違う
			var expected = new SampleObject("Aaa", 101);

			// Assert
			Assert.NotSame(expected, actual);
			// 等しくない
			Assert.NotEqual(expected, actual);
		}
	}
}
