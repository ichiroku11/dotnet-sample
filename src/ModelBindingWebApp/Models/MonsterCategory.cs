using Microsoft.AspNetCore.Mvc.Rendering;
using SampleLib;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Models;

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
	public static SelectListItem ToSelectListItem(this MonsterCategory category) {
		var displayName = category == MonsterCategory.None
			? "選択してください"
			: category.DisplayName();

		return new SelectListItem {
			Text = displayName,
			Value = category.ToString().ToLower(),
		};
	}
}
