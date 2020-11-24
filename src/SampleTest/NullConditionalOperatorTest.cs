using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	// null条件演算子のテスト
	public class NullConditionalOperatorTest {
		[Fact]
		public void Null条件演算子_何を試そう() {
			var value = false;
			Action action = () => value = true;
			action?.Invoke();

			Assert.True(value);

		}
	}
}
