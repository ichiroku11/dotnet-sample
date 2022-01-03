using Microsoft.AspNetCore.Mvc.Testing;
using MiscWebApi.Models;
using Xunit.Abstractions;

namespace MiscWebApi.Controllers.Test;

public partial class MonsterControllerTest : ControllerTestBase {
	private static readonly Monster _slime = new Monster {
		Id = 1,
		Name = "スライム",
	};

	public MonsterControllerTest(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
		: base(output, factory) {
	}
}
