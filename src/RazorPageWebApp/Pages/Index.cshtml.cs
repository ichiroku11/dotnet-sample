using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models;

namespace RazorPageWebApp.Pages;

public class IndexModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	public IList<Monster> Monsters { get; private set; } = default!;

	public void OnGet() {
		Monsters = _repository.QueryAll();
	}
}
