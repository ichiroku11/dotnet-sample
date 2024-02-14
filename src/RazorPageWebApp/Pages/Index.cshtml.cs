using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models;

namespace RazorPageWebApp.Pages;

public class IndexModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	public IList<Monster> Monsters { get; private set; } = new List<Monster>();

	[BindProperty(SupportsGet = true)]
	public string Query { get; set; } = "";

	public async Task OnGetAsync() {
		Monsters = await _repository.QueryAsync(new MonsterListQueryOption(Query));
	}
}
