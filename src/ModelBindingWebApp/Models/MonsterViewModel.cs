using Microsoft.AspNetCore.Mvc.Rendering;

namespace ModelBindingWebApp.Models;

public class MonsterViewModel {
	public IEnumerable<SelectListItem> CategorySelectListItems { get; init; } = null!;
	// POSTする部分を別モデルとする
	public MonsterFormModel Form { get; init; } = null!;
}
