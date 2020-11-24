using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	// null合体演算子のテスト
	public class NullCoalescingOperatorTest {
		[Fact]
		public void Null合体演算子_左辺がnullでない場合は左辺の値を返す() {
			var x = (int?)1;
			var y = -1;

			var result = x ?? y;
			// 以下と同じ
			//var result = x != null ? x : y; 

			Assert.Equal(1, result);
		}

		[Fact]
		public void Null合体演算子_左辺がnullの場合は右辺の値を返す() {
			var x = default(int?);
			var y = -1;

			var result = x ?? y;
			// 以下と同じ
			//var result = x != null ? x : y; 

			Assert.Equal(-1, result);
		}

		[Fact]
		public void Null合体演算子_左辺がnullの場合は右辺の値を返すその2() {
			var x = default(int?);
			var y = default(int?);
			var z = 0;

			var result = x ?? y ?? z;
			/*
			// 以下と同じ
			var result = x != null
				? x
				: y != null
					? y
					: z;
			*/

			Assert.Equal(0, result);
		}

		[Fact]
		public void Null合体演算子_評価順を確認する() {
			var count = 0;
			int? getX() {
				Assert.Equal(0, count);
				count++;
				return null;
			}

			int? getY() {
				Assert.Equal(1, count);
				count++;
				return null;
			}

			int? getZ() {
				Assert.Equal(2, count);
				count++;
				return 0;
			}

			// 前から順に評価される
			var result = getX() ?? getY() ?? getZ();

			Assert.Equal(0, result);
			Assert.Equal(3, count);
		}
	}
}
