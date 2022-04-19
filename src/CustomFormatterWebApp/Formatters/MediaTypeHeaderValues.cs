using Microsoft.Net.Http.Headers;

namespace CustomFormatterWebApp.Formatters;

public static class MediaTypeHeaderValues {
	public static readonly MediaTypeHeaderValue TextPlain
		= MediaTypeHeaderValue.Parse("text/plain").CopyAsReadOnly();

}
