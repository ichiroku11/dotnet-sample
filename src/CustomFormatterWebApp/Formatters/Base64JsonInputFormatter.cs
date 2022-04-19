using CustomFormatterWebApp.Helpers;
using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonInputFormatter : TextInputFormatter {
	public Base64JsonInputFormatter() {
		// エンコードやメディアタイプでフィルタされる様子
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedMediaTypes.Add(MediaTypeHeaderValues.TextPlain);
	}

	// 読み込み対象の型かどうか
	protected override bool CanReadType(Type type) {
		return type == typeof(Sample);
	}

	// リクエストのボディを読み込む
	public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
		using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
		var base64json = await reader.ReadToEndAsync();

		var model = Base64JsonSerializer.Deserialize<Sample>(base64json);

		return InputFormatterResult.Success(model);
	}
}
