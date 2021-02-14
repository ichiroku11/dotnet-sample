using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagHelperWebApp.TagHelpers {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-3.1
	[HtmlTargetElement(Attributes = nameof(If))]
	public class IfTagHelper : TagHelper {
		public bool If { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			if (!If) {
				// htmlを出力しない
				output.SuppressOutput();
			}
		}
	}
}
