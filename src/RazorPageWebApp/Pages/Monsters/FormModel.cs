using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Pages.Monsters;

public class FormModel {
	[Range(1, 999)]
	public int Id { get; set; }

	[Length(2, 10)]
	public string Name { get; set; } = "";
}
