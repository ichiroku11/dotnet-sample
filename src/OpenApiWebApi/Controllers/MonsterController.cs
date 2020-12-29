using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OpenApiWebApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenApiWebApi.Controllers {
	/// <summary>
	/// 
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	// OpenApiTags属性もあって紛らわしい
	[OpenApiTag(nameof(Monster), Description = "モンスター")]
	public class MonsterController : ControllerBase {
		private static readonly ConcurrentDictionary<int, Monster> _monsters
			= new ConcurrentDictionary<int, Monster>(
				new[] {
					new Monster {
						Id = 1,
						Name = "スライム"
					},
					new Monster {
						Id = 2,
						Name = "ドラキー"
					}
				}.ToDictionary(monster => monster.Id));

		// アクションメソッドのsummaryがOpenAPIに出力される
		/// <summary>
		/// <see cref="Monster"/>一覧を取得
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Monster> Get() {
			return _monsters.OrderBy(entry => entry.Key).Select(entry => entry.Value);
		}

		/// <summary>
		/// モンスターIDで<see cref="Monster"/>を取得
		/// </summary>
		/// <param name="id">モンスターID</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		// Typeを指定しなくても戻り値の型を特定してくれる様子
		//[ProducesResponseType(typeof(Monster), StatusCodes.Status200OK)]
		//[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
		public ActionResult<Monster> Get(int id) {
			if (!_monsters.TryGetValue(id, out var monster)) {
				return NotFound();
			}

			return monster;
		}

		// todo:
		/*
		// POST api/<MonsterController>
		[HttpPost]
		public void Post([FromBody] string value) {
		}

		// PUT api/<MonsterController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value) {
		}


		// DELETE api/<MonsterController>/5
		[HttpDelete("{id}")]
		public void Delete(int id) {
		}
		*/
	}
}
