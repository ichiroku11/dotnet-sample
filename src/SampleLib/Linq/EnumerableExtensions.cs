namespace SampleLib.Linq;

public static class EnumerableExtensions {
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

	/// <summary>
	/// 要素の順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	public static IEnumerable<(TSource Item, int Rank)> DenseRank<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull {

		var rank = source
			.Distinct()
			.Order()
			.Index()
			.ToDictionary(
				keySelector: item => item.Item,
				elementSelector: item => item.Index + 1);

		return source.Select(item => (item, rank[item]));
	}
}
