using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPageWebApp.Models;

namespace RazorPageWebApp.Pages;

public class IndexModel(MonsterRepository repository) : PageModel {
	private readonly MonsterRepository _repository = repository;

	public Task<IList<Monster>> QueryMonstersAsync()
		=> _repository.QueryAsync(new MonsterListQueryOption(Query));

	[BindProperty(SupportsGet = true)]
	public string Query { get; set; } = "";

	public void OnGet() {
	}
}
