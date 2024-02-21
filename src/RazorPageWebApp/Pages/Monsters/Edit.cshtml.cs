using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models.Monsters;
using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Pages.Monsters;

public class EditModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	[BindProperty]
	[Range(1, 999)]
	public int Id { get; set; }

	[BindProperty]
	[Length(2, 10)]
	public string Name { get; set; } = "";

	public async Task<IActionResult> OnGetAsync(int id) {
		var monster = await _repository.GetByIdAsync(id);
		if (monster is null) {
			return NotFound();
		}

		Id = monster.Id;
		Name = monster.Name;

		return Page();
	}

	public async Task<IActionResult> OnPostAsync() {
		// todo: Validation

		await _repository.UpdateAsync(new Monster(Id, Name));

		return RedirectToPage("Index");
	}

}
