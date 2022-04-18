using CustomFormatterWebApp.Helpers;
using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonInputFormatter : TextInputFormatter {
	public Base64JsonInputFormatter() {
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedMediaTypes.Add(MediaTypeHeaderValues.TextPlain);
	}

	protected override bool CanReadType(Type type) {
		return type == typeof(Sample);
	}

	public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
		// todo: try catch

		using var reader = new StreamReader(context.HttpContext.Request.Body, encoding);
		var base64json = await reader.ReadToEndAsync();

		var model = Base64JsonSerializer.Deserialize<Sample>(base64json);

		return InputFormatterResult.Success(model);
	}
}
