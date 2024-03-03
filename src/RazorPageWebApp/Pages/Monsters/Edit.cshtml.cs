using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models.Monsters;
using SampleLib.AspNetCore.Mvc.Filters;

namespace RazorPageWebApp.Pages.Monsters;

[TypeFilter(typeof(LoadModelStateAsyncPageFilter))]
[TypeFilter(typeof(SaveModelStateAsyncResultFilter))]
public class EditModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	private RedirectToPageResult RedirectToIndexPage() => RedirectToPage("Index");

	public async Task<IActionResult> OnGetAsync(int id) {
		var monster = await _repository.GetByIdAsync(id);
		if (monster is null) {
			return NotFound();
		}

		ViewData["formModel"] = new EditFormModel {
			Id = monster.Id,
			Name = monster.Name,
		};

		return Page();
	}

	public async Task<IActionResult> OnPostAsync(EditFormModel formModel) {
		if (!ModelState.IsValid) {
			return RedirectToPage();
		}

		await _repository.UpdateAsync(formModel.CreateMonster());

		return RedirectToIndexPage();
	}

	public async Task<IActionResult> OnPostDeleteAsync(int id) {
		await _repository.DeleteAsync(id);

		return RedirectToIndexPage();
	}
}
