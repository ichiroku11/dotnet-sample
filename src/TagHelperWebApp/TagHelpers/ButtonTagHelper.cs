using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TagHelperWebApp.TagHelpers {
	// Bootstrapのボタン
	public enum ButtonStyle {
		Primary,
		Secondary,
		// 省略
	}

	// Bootstrapのボタンスタイルを適用するTagHelper
	[HtmlTargetElement("button", Attributes = _styleAttributeName)]
	public class ButtonTagHelper : TagHelper {
		private const string _styleAttributeName = "bs-style";
		private readonly HtmlEncoder _encoder;

		public ButtonTagHelper(HtmlEncoder encoder) {
			_encoder = encoder;
		}

		[HtmlAttributeName(_styleAttributeName)]
		public ButtonStyle Style { get; set; }

		public override void Process(TagHelperContext context, TagHelperOutput output) {
			// ライブラリ側でclass属性を操作する場合がある
			// button要素ではないがinput要素だと"input-validation-error"が付与されたりする
			// その場合は文字列ではなくクラスが追加されるので、
			// output.Attributes["class"].Valueを文字列操作しない方がよい
			var classes = new[] { "btn", $"btn-{Style.ToString().ToLower()}" };
			foreach (var @class in classes) {
				output.AddClass(@class, _encoder);
			}
		}
	}
}
