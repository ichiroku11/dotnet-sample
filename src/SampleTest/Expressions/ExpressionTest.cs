using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace SampleTest.Expressions {
	public class ExpressionTest {
		// 参考
		// https://www.slideshare.net/Fujiwo/20141026-c-c-expression-tree
		[Fact]
		public void Expression_足し算を構築する() {
			// Expression<Func<int, int, int>> lambda = (x, y) => x + y;
			// var func = lambda.Compile();
			Func<int, int, int> createFunc() {
				var x = Expression.Parameter(typeof(int));
				var y = Expression.Parameter(typeof(int));
				var add = Expression.Add(x, y);

				var lambda = Expression.Lambda<Func<int, int, int>>(add, x, y);

				return lambda.Compile();
			}

			var func = createFunc();
			var result = func(1, 2);

			Assert.Equal(3, result);
		}

		// テスト用
		private class Item {
			public int Value { get; set; }
			public int GetValue(int value) => value + 1;
		}

		[Fact]
		public void Expression_プロパティアクセスを構築する() {
			// Expression<Func<Item, int>> expression = item => item.Value;
			// var func = lambda.Compile();
			Func<Item, int> createFunc() {
				var target = Expression.Parameter(typeof(Item));
				var property = Expression.Property(target, "Value");

				var lambda = Expression.Lambda<Func<Item, int>>(property, target);

				return lambda.Compile();
			}

			var func = createFunc();

			var item = new Item { Value = 1 };
			var result = func(item);

			Assert.Equal(item.Value, result);
		}

		[Fact]
		public void Expression_メソッド呼び出しを構築する() {
			// Expression<Func<Item, int, int>> lambda = (item, param) => item.GetValue(param);
			// var func = lambda.Compile();
			Func<Item, int, int> createFunc() {
				var target = Expression.Parameter(typeof(Item));
				var param = Expression.Parameter(typeof(int));
				var method = Expression.Call(target, typeof(Item).GetMethod("GetValue"), param);

				var lambda = Expression.Lambda<Func<Item, int, int>>(method, target, param);

				return lambda.Compile();
			}

			var func = createFunc();

			var item = new Item();
			var result = func(item, 2);

			Assert.Equal(3, result);
		}
	}
}
