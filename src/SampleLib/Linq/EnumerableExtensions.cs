using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleLib.Linq;

public static class EnumerableExtensions {
	/// <summary>
	/// 一意の要素を取得
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <param name="source"></param>
	/// <param name="compareKeySelector"></param>
	/// <returns></returns>
	public static IEnumerable<TSource> Distinct<TSource, TKey>(
		this IEnumerable<TSource> source,
		Func<TSource, TKey> compareKeySelector)
		where TSource : notnull
		where TKey : notnull
		=> source.Distinct(new EqualityComparer<TSource, TKey>(compareKeySelector));

	/// <summary>
	/// 2つのシーケンスが等しいか
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	/// <param name="first"></param>
	/// <param name="second"></param>
	/// <param name="compareKeySelector"></param>
	/// <returns></returns>
	public static bool SequenceEqual<TSource, TKey>(
		this IEnumerable<TSource> first,
		IEnumerable<TSource> second,
		Func<TSource, TKey> compareKeySelector)
		where TSource : notnull
		where TKey : notnull
		=> first.SequenceEqual(second, new EqualityComparer<TSource, TKey>(compareKeySelector));
}
