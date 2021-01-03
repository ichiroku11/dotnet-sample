using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SampleLib {
	/// <summary>
	/// EqualityComparerの実装
	/// </summary>
	/// <typeparam name="TElement"></typeparam>
	/// <typeparam name="TKey"></typeparam>
	public class EqualityComparer<TElement, TKey> : EqualityComparer<TElement> {
		private readonly Func<TElement, TKey> _keySelector;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keySelector"></param>
		public EqualityComparer(Func<TElement, TKey> keySelector) {
			_keySelector = keySelector;
		}

		/// <summary>
		/// 指定したオブジェクトが等しいかどうか
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public override bool Equals([AllowNull]TElement x, [AllowNull]TElement y) {
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
		public override int GetHashCode([DisallowNull]TElement obj) {
			if (obj == null) {
				return 0;
			}

			return _keySelector(obj).GetHashCode();
		}
	}
}
