using CustomFormatterWebApp.Helpers;
using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonOutputFormatter : TextOutputFormatter {
	public Base64JsonOutputFormatter() {
		// エンコードやメディアタイプでフィルタされる様子
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedMediaTypes.Add(MediaTypeHeaderValues.TextPlain);
	}

	// 書き込み対象の型かどうか
	protected override bool CanWriteType(Type? type) {
		if (type is null) {
			return false;
		}

		return type == typeof(Sample);
	}

	// レスポンスのボディに書き込む
	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
		if (context.Object is not Sample model) {
			return;
		}

		var base64json = Base64JsonSerializer.Serialize(model);

		await context.HttpContext.Response.WriteAsync(base64json, selectedEncoding);
	}
}
