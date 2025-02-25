namespace SampleLib.Linq;

public static class EnumerableExtensions {
	private static IOrderedEnumerable<TSource> Order<TSource>(this IEnumerable<TSource> source, bool ascending)
		where TSource : notnull
		=> ascending ? source.Order() : source.OrderDescending();

	private static IEnumerable<(TSource Item, int Rank)> DenseRankCore<TSource>(this IEnumerable<TSource> source, bool ascending)
		where TSource : notnull {

		var rank = source
			.Distinct()
			.Order(ascending)
			.Index()
			.ToDictionary(
				keySelector: item => item.Item,
				elementSelector: item => item.Index + 1);

		return source.Select(item => (item, rank[item]));
	}

	/// <summary>
	/// 要素を昇順で並び替えた順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <remarks><paramref name="source"/>は並び替えられない</remarks>
	public static IEnumerable<(TSource Item, int Rank)> DenseRank<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.DenseRankCore(true);

	/// <summary>
	/// 要素を降順で並び替えた順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <remarks><paramref name="source"/>は並び替えられない</remarks>
	public static IEnumerable<(TSource Item, int Rank)> DenseRankDescending<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.DenseRankCore(false);
}
