using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagHelperWebApp.TagHelpers {
	// 参考
	// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-5.0#avoid-tag-helper-conflicts
	public abstract class ParagraphTagHelper : TagHelper {
		private readonly string _content;

		public ParagraphTagHelper(string content) {
			_content = content;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			// todo: 属性も追加

			// コンテンツを追加
			if (!output.Content.IsModified) {
				// 元々のコンテンツを追加
				var content = await output.GetChildContentAsync();
				output.Content.AppendHtml(content.GetContent());
			}
			output.Content.AppendHtml($@"<span>:{_content}</span>");
		}
	}

	[HtmlTargetElement("p", Attributes = _content)]
	public class FirstParagraphTagHelper : ParagraphTagHelper {
		private const string _content = "First";

		public FirstParagraphTagHelper() : base(_content) {
		}

		public override int Order => 1;

	}

	[HtmlTargetElement("p", Attributes = _content)]
	public class SecondParagraphTagHelper : ParagraphTagHelper {
		private const string _content = "Second";

		public SecondParagraphTagHelper() : base(_content) {
		}

		public override int Order => 2;
	}
}
