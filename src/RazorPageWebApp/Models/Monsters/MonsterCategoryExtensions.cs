using Microsoft.AspNetCore.Mvc.Rendering;
using SampleLib;

namespace RazorPageWebApp.Models.Monsters;

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
