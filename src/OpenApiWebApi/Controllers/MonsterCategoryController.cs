using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OpenApiWebApi.Models;

namespace OpenApiWebApi.Controllers;

/// <summary>
/// モンスターカテゴリコントローラ
/// </summary>
[Route("api/[controller]")]
[ApiController]
[ApiConventionType(typeof(ApiConventions))]
[OpenApiTag("MonsterCategory", Description = "モンスターカテゴリ")]
public class MonsterCategoryController : ControllerBase {
	/// <summary>
	/// モンスターカテゴリを取得
	/// </summary>
	/// <param name="category"></param>
	/// <returns></returns>
	[HttpGet("{category}")]
	[ApiConventionMethod(typeof(ApiConventions), nameof(ApiConventions.Get))]
	public ActionResult<MonsterCategoryResponse> Get(MonsterCategory category) {
		if (category == MonsterCategory.Unknown) {
			return BadRequest();
		}

		return new MonsterCategoryResponse {
			Category = category,
			// .NET 8のSampleLibとNSwag.AspNetCore 13.20.0でエラーになるため
			// 一旦SampleLibの依存をなくす
			CategoryName = category switch {
				MonsterCategory.Slime => "スライム系",
				MonsterCategory.Animal => "けもの系",
				MonsterCategory.Fly => "鳥系",
				_ => "",
			},
		};
	}
}
