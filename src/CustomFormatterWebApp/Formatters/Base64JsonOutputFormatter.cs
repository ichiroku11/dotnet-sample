using CustomFormatterWebApp.Helpers;
using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonOutputFormatter : TextOutputFormatter {
	public Base64JsonOutputFormatter() {
		SupportedEncodings.Add(Encoding.UTF8);
		SupportedMediaTypes.Add(MediaTypeHeaderValues.TextPlain);
	}

	protected override bool CanWriteType(Type? type) {
		if (type is null) {
			return false;
		}

		return type == typeof(Sample);
	}

	public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
		if (context.Object is not Sample model) {
			// todo: error?
			return;
		}

		var base64json = Base64JsonSerializer.Serialize(model);

		await context.HttpContext.Response.WriteAsync(base64json, selectedEncoding);
	}
}
