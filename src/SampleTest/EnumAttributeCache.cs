using System;
using System.Collections.Generic;
using System.Text;

namespace SampleTest {
	// Enum値のAttributeのキャッシュ
	public static class EnumAttributeCache<TEnum, TAttribute>
		where TEnum : Enum
		where TAttribute : Attribute {

		// 遅延実行する
		private static readonly Lazy<Dictionary<TEnum, TAttribute>> _attributes
			= new Lazy<Dictionary<TEnum, TAttribute>>(EnumHelper.GetAttributes<TEnum, TAttribute>);

		// 属性を取得
		public static TAttribute Get(TEnum @enum) => _attributes.Value[@enum];
	}
}
