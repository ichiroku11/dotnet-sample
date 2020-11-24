using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest.Collections {
	public class HashSetTest {
		private class Sample {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		[Fact]
		public void Contains_参照型の同じインスタンスならプロパティの値が変わっても含まれる() {
			// Arrange
			var sample = new Sample { Id = 1, Name = "1" };
			var set = new HashSet<Sample> { sample };

			// Act
			var containsBefore = set.Contains(sample);

			// 同じインスタンスであればプロパティの値が変わっても含まれる
			sample.Id = 2;
			sample.Name = "2";
			var containsAfter = set.Contains(sample);

			// Assert
			Assert.True(containsBefore);
			Assert.True(containsAfter);
		}

		[Fact]
		public void Contains_参照型の異なるインスタンスならプロパティが同じでも含まれない() {
			// Arrange
			var sample = new Sample { Id = 1, Name = "1" };
			var set = new HashSet<Sample> { sample };

			// Act
			// 異なるインスタンスは含まれない
			var contains = set.Contains(new Sample { Id = 1, Name = "1" });

			// Assert
			Assert.False(contains);
		}
	}
}
