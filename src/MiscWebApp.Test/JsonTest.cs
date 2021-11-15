using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace MiscWebApp.Test;

public class JsonTest : TestBase {
	private static readonly JsonSerializerOptions _jsonSerializerOptions = Startup.JsonSerializerOptions;

	public JsonTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}

	[Fact]
	public async Task ReadFromJsonAsyncとWriteAsJsonAsyncを確認する() {
		// Arrange
		var expected = new Startup.Sample { Value = 1 };
		var request = new HttpRequestMessage(HttpMethod.Post, "/json") {
			Content = JsonContent.Create(expected, options: _jsonSerializerOptions)
		};

		// Act
		var response = await SendAsync(request);
		var actual = await response.Content.ReadFromJsonAsync<Startup.Sample>(_jsonSerializerOptions);

		// Assert
		Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		Assert.Equal(1, actual.Value);
	}
}
