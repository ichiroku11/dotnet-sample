using Microsoft.AspNetCore.Mvc.Testing;
using MiscWebApi.Models;
using Xunit.Abstractions;

namespace MiscWebApi.Controllers.Test;

public partial class MonsterControllerTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
	: ControllerTestBase(output, factory) {
	private static readonly Monster _slime = new() {
		Id = 1,
		Name = "スライム",
	};
}
