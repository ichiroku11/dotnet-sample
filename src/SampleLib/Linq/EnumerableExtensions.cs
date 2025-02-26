namespace SampleLib.Linq;

public static class EnumerableExtensions {
	private static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(
		this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, bool ascending)
		where TSource : notnull
		=> ascending
			? source.OrderBy(keySelector)
			: source.OrderByDescending(keySelector);

	private static IEnumerable<(TSource Item, int Rank)> DenseRankCore<TSource>(this IEnumerable<TSource> source, bool ascending)
		where TSource : notnull {

		var rank = source
			.Distinct()
			.OrderBy(item => item, ascending)
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
	/// <remarks>
	/// 同じ順位がある場合でも順位は飛び飛びにならない
	/// <paramref name="source"/>は並び替えられない
	/// </remarks>
	public static IEnumerable<(TSource Item, int Rank)> DenseRank<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.DenseRankCore(true);

	/// <summary>
	/// 要素を降順で並び替えた順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <remarks>
	/// 同じ順位がある場合でも順位は飛び飛びにならない
	/// <paramref name="source"/>は並び替えられない
	/// </remarks>
	public static IEnumerable<(TSource Item, int Rank)> DenseRankDescending<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.DenseRankCore(false);

	private static IEnumerable<(TSource Item, int Rank)> RankCore<TSource>(this IEnumerable<TSource> source, bool ascending)
		where TSource : notnull {

		var rank = new Dictionary<TSource, int>();
		var _ = source
			.CountBy(item => item)
			.OrderBy(item => item.Key, ascending)
			// seedとaccumlatedは順位を表す
			.Aggregate(
				// 最初の順位：1位
				seed: 1,
				func: (accumlated, item) => {
					// 集計しながら順位を登録していく
					rank[item.Key] = accumlated;

					// 次の順位：今の順位 + 今の順位の個数
					return accumlated + item.Value;
				});

		return source.Select(item => (item, rank[item]));
	}

	/// <summary>
	/// 要素を昇順で並び替えた順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <remarks>
	/// 同じ順位がある場合、それ以降の順位は詰められない（順位が飛び飛びになる）
	/// <paramref name="source"/>は並び替えられない
	/// </remarks>
	public static IEnumerable<(TSource Item, int Rank)> Rank<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.RankCore(true);

	/// <summary>
	/// 要素を降順で並び替えた順位を含むタプルのシーケンスを取得する
	/// </summary>
	/// <typeparam name="TSource"></typeparam>
	/// <param name="source"></param>
	/// <returns></returns>
	/// <remarks>
	/// 同じ順位がある場合、それ以降の順位は詰められない（順位が飛び飛びになる）
	/// <paramref name="source"/>は並び替えられない
	/// </remarks>
	public static IEnumerable<(TSource Item, int Rank)> RankDescending<TSource>(this IEnumerable<TSource> source)
		where TSource : notnull
		=> source.RankCore(false);
}
