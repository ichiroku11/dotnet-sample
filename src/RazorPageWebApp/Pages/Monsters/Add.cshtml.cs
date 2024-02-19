using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models.Monsters;
using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Pages.Monsters;

public class AddModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	[BindProperty]
	[Range(1, 999)]
	public int Id { get; set; }

	[BindProperty]
	[Length(2, 10)]
	public string Name { get; set; } = "";

	public void OnGet() {
	}

	public async Task<IActionResult> OnPostAsync() {
		await _repository.AddAsync(new Monster(Id, Name));

		return RedirectToPage("Index");
	}
}
