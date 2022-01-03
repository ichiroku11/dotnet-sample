using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Models;

public class UserUpdateCommand {
	[Display(Name = "メールアドレス")]
	[Required]
	public string MailAddress { get; init; } = "";

	[Display(Name = "表示名")]
	public string DisplayName { get; init; } = "";
}
