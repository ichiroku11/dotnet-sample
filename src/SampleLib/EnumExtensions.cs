using System.ComponentModel.DataAnnotations;

namespace SampleLib;

public static class EnumExtensions {
	/// <summary>
	/// 表示名を取得
	/// </summary>
	/// <typeparam name="TEnum"></typeparam>
	/// <param name="value"></param>
	/// <returns></returns>
	public static string? DisplayName<TEnum>(this TEnum value) where TEnum : Enum
		=> EnumAttributeCache<TEnum, DisplayAttribute>.Get(value)?.Name;
}
