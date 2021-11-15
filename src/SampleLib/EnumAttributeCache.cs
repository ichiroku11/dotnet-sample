using System;
using System.Collections.Generic;
using System.Text;

namespace SampleLib;

/// <summary>
/// Enum値のAttributeのキャッシュ
/// </summary>
/// <typeparam name="TEnum"></typeparam>
/// <typeparam name="TAttribute"></typeparam>
public static class EnumAttributeCache<TEnum, TAttribute>
	where TEnum : Enum
	where TAttribute : Attribute {

	// 遅延実行する
	private static readonly Lazy<Dictionary<TEnum, TAttribute>> _attributes
		= new Lazy<Dictionary<TEnum, TAttribute>>(EnumHelper.GetAttributes<TEnum, TAttribute>);

	/// <summary>
	/// 属性を取得
	/// </summary>
	/// <param name="enum"></param>
	/// <returns></returns>
	public static TAttribute Get(TEnum @enum) => _attributes.Value[@enum];
}
