using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPageWebApp.Pages;

public class AddModel : PageModel {
	[BindProperty]
	public int Id { get; set; }

	[BindProperty]
	public string Name { get; set; } = "";

	public void OnGet() {
	}

	public Task OnPostAsync() {

		// todo:

		return Task.CompletedTask;
	}
}
