using System.Diagnostics.CodeAnalysis;

namespace SampleLib;

/// <summary>
/// EqualityComparerの実装
/// </summary>
/// <typeparam name="TElement"></typeparam>
/// <typeparam name="TKey"></typeparam>
/// <param name="keySelector"></param>
public class EqualityComparer<TElement, TKey>(Func<TElement, TKey> keySelector) : EqualityComparer<TElement>
	where TElement : notnull
	where TKey : notnull {
	private readonly Func<TElement, TKey> _keySelector = keySelector;

	/// <summary>
	/// 指定したオブジェクトが等しいかどうか
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns></returns>
	public override bool Equals([AllowNull] TElement x, [AllowNull] TElement y) {
		// 2つが同じインスタンスなら等しい
		if (ReferenceEquals(x, y)) {
			return true;
		}

		// どちかがnullなら等しくない
		if (x == null || y == null) {
			return false;
		}

		return _keySelector(x).Equals(_keySelector(y));
	}

	/// <summary>
	/// 指定したオブジェクトのハッシュコードを取得
	/// </summary>
	/// <param name="obj"></param>
	/// <returns></returns>
	public override int GetHashCode([DisallowNull] TElement obj) {
		return _keySelector(obj).GetHashCode();
	}
}
