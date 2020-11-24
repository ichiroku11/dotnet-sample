using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleTest.Linq {
	public static class EnumerableExtensions {
		// 一意の要素を取得
		public static IEnumerable<TSource> Distinct<TSource, TKey>(
			this IEnumerable<TSource> source,
			Func<TSource, TKey> compareKeySelector)
			=> source.Distinct(new EqualityComparer<TSource, TKey>(compareKeySelector));

		// 2つのシーケンスが等しいか
		public static bool SequenceEqual<TSource, TKey>(
			this IEnumerable<TSource> first,
			IEnumerable<TSource> second,
			Func<TSource, TKey> compareKeySelector)
			=> first.SequenceEqual(second, new EqualityComparer<TSource, TKey>(compareKeySelector));
	}
}
