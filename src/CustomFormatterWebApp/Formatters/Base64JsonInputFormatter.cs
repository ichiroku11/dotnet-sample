using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonInputFormatter : TextInputFormatter {
	public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
		throw new NotImplementedException();
	}
}
