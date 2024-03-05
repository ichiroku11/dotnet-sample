using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models.Monsters;
using SampleLib.AspNetCore.Mvc.Filters;

namespace RazorPageWebApp.Pages.Monsters;

[TypeFilter(typeof(LoadModelStateAsyncPageFilter))]
[TypeFilter(typeof(SaveModelStateAsyncResultFilter))]
public class AddModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	public void OnGet() {
	}

	public async Task<IActionResult> OnPostAsync(EditFormModel formModel) {
		if (!ModelState.IsValid) {
			return RedirectToPage();
		}

		await _repository.AddAsync(formModel.ToMonster());

		return RedirectToPage("Index");
	}
}
