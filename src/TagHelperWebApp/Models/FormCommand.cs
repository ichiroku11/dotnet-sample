namespace TagHelperWebApp.Models;

public class FormCommand {
	public string Text { get; set; } = "";

	public bool CheckBox { get; set; }

	public int Radio { get; set; }

	public int Select { get; set; }

	public string MultilineText { get; set; } = "";
}
