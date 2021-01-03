using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Controllers {
	/// <summary>
	/// 
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiConventionType(typeof(ApiConventions))]
	[OpenApiTag("MonsterCategory", Description = "モンスターカテゴリ")]
	public class MonsterCategoryController : ControllerBase {

	}
}
