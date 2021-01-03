using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SampleLib {
	public static class EnumHelper {
		/// <summary>
		/// TEnum=>TAttributeのDictionaryを取得
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <typeparam name="TAttribute"></typeparam>
		/// <returns></returns>
		public static Dictionary<TEnum, TAttribute> GetAttributes<TEnum, TAttribute>()
			where TEnum : Enum
			where TAttribute : Attribute {
			return typeof(TEnum)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					field => (TEnum)field.GetValue(null),
					field => field.GetCustomAttributes<TAttribute>().FirstOrDefault());
		}

		/// <summary>
		/// TEnum一覧の取得
		/// </summary>
		/// <typeparam name="TEnum"></typeparam>
		/// <returns></returns>
		public static IEnumerable<TEnum> GetValues<TEnum>() => Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
	}
}
