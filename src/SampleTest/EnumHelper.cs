using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SampleTest {
	public static class EnumHelper {
		// TEnum=>TAttributeのDictionaryを取得
		public static Dictionary<TEnum, TAttribute> GetAttributes<TEnum, TAttribute>()
			where TEnum : Enum
			where TAttribute : Attribute {
			return typeof(TEnum)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					field => (TEnum)field.GetValue(null),
					field => field.GetCustomAttributes<TAttribute>().FirstOrDefault());
		}

		// TEnum一覧の取得
		public static IEnumerable<TEnum> GetValues<TEnum>() => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
	}
}
