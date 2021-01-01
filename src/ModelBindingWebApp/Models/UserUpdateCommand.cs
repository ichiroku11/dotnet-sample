using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	public class UserUpdateCommand {
		[Display(Name = "メールアドレス")]
		[Required]
		public string MailAddress { get; set; }

		[Display(Name = "表示名")]
		public string DisplayName { get; set; }
	}
}
