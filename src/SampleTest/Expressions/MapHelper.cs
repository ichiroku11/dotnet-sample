using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SampleTest.Expressions {
	public static class MapHelper {
		// AutoMapperのようなマッピングメソッドを生成する
		public static Func<TSource, TResult> CreateMapper<TSource, TResult>() {
			// 関数パラメータの宣言
			var source = Expression.Parameter(typeof(TSource), "source");

			// コピー先変数の宣言
			var result = Expression.Variable(typeof(TResult), "result");

			// コピー先インスタンスを変数に代入する式
			var assignToResult = Expression.Assign(result, Expression.New(typeof(TResult)));

			// プロパティの代入式
			var assignProps = typeof(TSource)
				// コピー元：読み取りできるパブリックなインスタンスプロパティが対象
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(prop => prop.CanRead)
				// コピー先：書き込みできるパブリックなインスタンスプロパティが対象
				.Select(prop => new {
					From = prop,
					To = typeof(TResult).GetProperty(prop.Name, BindingFlags.Public | BindingFlags.Instance)
				})
				.Where(entry => entry.To != null && entry.To.CanWrite)
				.Select(entry =>
					// コピー先のプロパティにコピー元のプロパティの値を代入する式
					Expression.Assign(
						Expression.Property(result, entry.To),
						Expression.Property(source, entry.From)));

			// ブロックに格納する式の本体
			var expressions = new List<Expression> {
				assignToResult,
			};
			expressions.AddRange(assignProps);
			// これが変数をreturnする式っぽい
			expressions.Add(result);

			// 式の引数と式の本体のブロック
			var block = Expression.Block(new[] { result }, expressions);

			var lambda = Expression.Lambda<Func<TSource, TResult>>(block, source);

			return lambda.Compile();
		}
	}
}
