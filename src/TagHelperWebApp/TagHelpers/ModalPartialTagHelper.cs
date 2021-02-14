using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Buffers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TagHelperWebApp.Models;

namespace TagHelperWebApp.TagHelpers {
	public class ModalPartialTagHelper : TagHelper {
		private readonly ModalPartialViewModel _model = new ModalPartialViewModel();
		private readonly PartialTagHelper _inner;

		public ModalPartialTagHelper(ICompositeViewEngine viewEngine, IViewBufferScope viewBufferScope) {
			_inner = new PartialTagHelper(viewEngine, viewBufferScope) {
				Name = "_Modal",
				Model = _model,
			};
		}

		// PartialTagHelperにViewContextが必要みたい
		// これがないとNullReferenceException
		[HtmlAttributeNotBound]
		[ViewContext]
		public ViewContext ViewContext {
			get => _inner.ViewContext;
			set => _inner.ViewContext = value;
		}

		// モーダルのID
		public string Id {
			get => _model.Id;
			set => _model.Id = value;
		}

		// モーダルのタイトル
		public string Title {
			get => _model.Title;
			set => _model.Title = value;
		}

		public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
			// 子コンテンツをモーダルのボディ用htmlとする
			_model.Body = await output.GetChildContentAsync();

			await _inner.ProcessAsync(context, output);
		}
	}
}
