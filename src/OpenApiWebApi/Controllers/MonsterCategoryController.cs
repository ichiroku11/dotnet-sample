using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OpenApiWebApi.Models;
using SampleLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
			CategoryName = category.DisplayName(),
		};
	}
}
