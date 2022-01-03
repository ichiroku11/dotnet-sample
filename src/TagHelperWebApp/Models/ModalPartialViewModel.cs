using Microsoft.AspNetCore.Html;

namespace TagHelperWebApp.Models;

public class ModalPartialViewModel {
	public string Id { get; set; } = "";
	public string Title { get; set; } = "";
	public IHtmlContent? Body { get; set; }
}
