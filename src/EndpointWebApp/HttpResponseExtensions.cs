using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndpointWebApp {
	public static class HttpResponseExtensions {
		public static Task WriteLineAsync(this HttpResponse response, string text)
			=> response.WriteAsync($"{text}{Environment.NewLine}");
	}
}
