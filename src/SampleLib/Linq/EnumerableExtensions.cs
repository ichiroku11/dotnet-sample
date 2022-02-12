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
}
