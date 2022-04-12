using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;

namespace CustomFormatterWebApp.Formatters;

public class Base64JsonOutputFormatter : TextOutputFormatter {
	public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
		throw new NotImplementedException();
	}
}
