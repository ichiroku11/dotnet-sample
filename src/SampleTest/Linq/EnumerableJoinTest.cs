using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableJoinTest {
		[Fact]
		public void Join_内部結合のような動きを確認する() {
			// Arrange
			var outers = new[] {
				new { Id = 1, Value1 = "a" },
				new { Id = 2, Value1 = "b" },
			};
			var inners = new[] {
				new { Id = 2, Value2 = "X" },
				new { Id = 3, Value2 = "Y" },
			};

			// Act
			// outers、innersそれぞれのIDで内部結合して結果を取得する
			var results = outers.Join(inners,
				outer => outer.Id,
				inner => inner.Id,
				(outer, inner) => new { outer.Id, outer.Value1, inner.Value2 });

			// Assert
			Assert.Single(results);
			Assert.Collection(results,
				result => {
					Assert.Equal(2, result.Id);
					Assert.Equal("b", result.Value1);
					Assert.Equal("X", result.Value2);
				});
		}
	}
}
