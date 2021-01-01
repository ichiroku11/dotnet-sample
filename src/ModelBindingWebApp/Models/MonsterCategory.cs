using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	public enum MonsterCategory {
		None = 0,

		[Display(Name = "スライム系")]
		Slime,

		[Display(Name = "けもの系")]
		Animal,

		[Display(Name = "鳥系")]
		Fly,
	}

	public static class MonsterCategoryExtensions {
		private static readonly Dictionary<MonsterCategory, DisplayAttribute> _displayAttributes
			= typeof(MonsterCategory)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					field => (MonsterCategory)field.GetValue(null),
					field => field.GetCustomAttributes<DisplayAttribute>().FirstOrDefault());

		public static string GetDisplayName(this MonsterCategory category)
			=> _displayAttributes[category]?.Name;

		public static SelectListItem ToSelectListItem(this MonsterCategory category) {
			var displayName = category == MonsterCategory.None
				? "選択してください"
				: category.GetDisplayName();

			return new SelectListItem {
				Text = displayName,
				Value = category.ToString().ToLower(),
			};
		}
	}
}
