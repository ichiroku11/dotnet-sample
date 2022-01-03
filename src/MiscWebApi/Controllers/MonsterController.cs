using Microsoft.AspNetCore.Mvc;
using MiscWebApi.Models;

namespace MiscWebApi.Controllers;

// 参考
// ASP.NET Core を使って Web API を作成する | Microsoft Docs
// https://docs.microsoft.com/ja-jp/aspnet/core/web-api/?view=aspnetcore-5.0
// チュートリアル: ASP.NET Core で Web API を作成する | Microsoft Docs
// https://docs.microsoft.com/ja-jp/aspnet/core/tutorials/first-web-api?view=aspnetcore-5.0&tabs=visual-studio
// ASP.NET Core Web API のコントローラー アクションの戻り値の型Controller action return types in ASP.NET Core web API | Microsoft Docs
// https://docs.microsoft.com/ja-jp/aspnet/core/web-api/action-return-types?view=aspnetcore-5.0

[Route("api/[controller]")]
[ApiController]
[ApiExplorerSettings(GroupName = "Monster")]
public class MonsterController : ControllerBase {
	private static readonly Dictionary<int, Monster> _monsters
		= new[] {
				new Monster { Id = 1, Name = "スライム" },
				new Monster { Id = 2, Name = "ドラキー" },
		}.ToDictionary(monster => monster.Id);


	// 何か時間がかかる処理
	private static Task ActionAsync() => Task.CompletedTask;

	// 一覧を取得
	// ~/api/monster
	[HttpGet]
	public async Task<IEnumerable<Monster>> GetAsync() {
		await ActionAsync();
		return _monsters.Values.OrderBy(monster => monster.Id);
	}

	// 単体を取得
	// ~/api/monster/{id}
	[HttpGet("{id}")]
	public async Task<ActionResult<Monster>> GetByIdAsync(int id) {
		await ActionAsync();

		if (!_monsters.TryGetValue(id, out var monster)) {
			return NotFound();
		}

		return monster;
	}

	// FromQuery属性
	// ~/api/monster/query
	[HttpGet("query")]
	public async Task<ActionResult<Monster>> GetQueryAsync([FromQuery] Monster monster) {
		await ActionAsync();

		return monster;
	}

	// FromRoute属性
	// ~/api/monster/route
	[HttpGet("route/{id}/{name}")]
	public async Task<ActionResult<Monster>> GetRouteAsync([FromRoute] Monster monster) {
		await ActionAsync();

		return monster;
	}

	// FromXxx属性：なし
	// ~/api/monster
	[HttpPost]
	public async Task<ActionResult<Monster>> PostAsync(Monster monster) {
		await ActionAsync();

		return monster;
	}

	// FromForm属性：POSTされたフォームから値を取得する
	// ~/api/monster/form
	[HttpPost("form")]
	public async Task<ActionResult<Monster>> PostFormAsync([FromForm] Monster monster) {
		await ActionAsync();

		return monster;
	}

	// FromBody属性：POSTされたリクエストボディから値を取得する
	// Consumes属性がない場合、リクエストヘッダにContentTypeが必要
	// ~/api/monster/body
	[HttpPost("body")]
	//[Consumes("application/json")]
	public async Task<ActionResult<Monster>> PostBodyAsync([FromBody] Monster monster) {
		await ActionAsync();

		return monster;
	}

	// FromBody属性：POSTされたリクエストボディから値を取得する
	// Consumes属性があっても、リクエストヘッダに一致するContentTypeが必要
	// ~/api/monster/body/json
	[HttpPost("body/json")]
	[Consumes("application/json")]
	public async Task<ActionResult<Monster>> PostBodyJsonAsync([FromBody] Monster monster) {
		await ActionAsync();

		return monster;
	}
}
