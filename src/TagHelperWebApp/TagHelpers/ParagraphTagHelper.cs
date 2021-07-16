using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagHelperWebApp.TagHelpers {
	public abstract class ParagraphTagHelper : TagHelper {
		private readonly string _content;

		public ParagraphTagHelper(string content) {
			_content = content;
		}

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			// todo: コンテンツが消えてしまう
			// todo: 属性も追加
			output.Content.Append($":{_content}");

			return Task.CompletedTask;
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
