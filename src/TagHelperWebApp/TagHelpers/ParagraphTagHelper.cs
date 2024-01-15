using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TagHelperWebApp.TagHelpers;

// 参考
// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/views/tag-helpers/authoring?view=aspnetcore-5.0#avoid-tag-helper-conflicts
public abstract class ParagraphTagHelper(string content) : TagHelper {
	private readonly string _content = content;

	public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
		// class属性を追加
		// 以下だと変更されない
		//output.Attributes.Add(new TagHelperAttribute("class", _content.ToLower()));
		var value = _content.ToLower();
		if (output.Attributes.TryGetAttribute("class", out var attribute)) {
			value = $"{attribute.Value} {_content.ToLower()}";
		}
		output.Attributes.SetAttribute("class", value);

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
