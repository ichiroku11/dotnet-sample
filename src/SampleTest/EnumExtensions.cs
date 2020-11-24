using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SampleTest {
	public static class EnumExtensions {
		// 表示名を取得
		public static string DisplayName<TEnum>(this TEnum value) where TEnum : Enum
			=> EnumAttributeCache<TEnum, DisplayAttribute>.Get(value)?.Name;
	}
}
