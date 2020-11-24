using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class ObjectTest {
		[Fact]
		public void ReferenceEquals_同じインスタンスならtrue() {
			// 同じインスタンスの比較はtrue
			var left = new object();
			var right = left;
			Assert.True(ReferenceEquals(left, right));
		}

		[Fact]
		public void ReferenceEquals_nullとnullならtrue() {
			// nullとnullの比較はtrue
			Assert.True(ReferenceEquals(null, null));
		}

		[Fact]
		public void ReferenceEquals_あるオブジェクトとnullはfalse() {
			// nullとの比較はfalse
			Assert.False(ReferenceEquals(new object(), null));
			Assert.False(ReferenceEquals(null, new object()));
		}

		[Fact]
		public void ReferenceEquals_値型の比較は常にfalse() {
			// ボックス化されるから、値型の比較は常にfalse
			Assert.False(ReferenceEquals(1, 1));
			Assert.False(ReferenceEquals(1L, 1L));
		}
	}
}
