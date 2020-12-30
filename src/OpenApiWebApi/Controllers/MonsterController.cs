using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using OpenApiWebApi.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
		// 最大ID
		private static int _maxId = 2;


		// アクションメソッドのsummaryがOpenAPIに出力される
		/// <summary>
		/// モンスター一覧を取得
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Monster> Get() {
			return _monsters.OrderBy(entry => entry.Key).Select(entry => entry.Value);
		}

		/// <summary>
		/// モンスターIDでモンスターを取得
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

		/// <summary>
		/// モンスターを追加
		/// </summary>
		/// <param name="request">モンスター追加リクエスト</param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public ActionResult<Monster> Post([FromBody] MonsterAddRequest request) {
			var id = Interlocked.Increment(ref _maxId);
			var monster = new Monster {
				Id = id,
				Name = request.Name,
			};

			if (!_monsters.TryAdd(id, monster)) {
				throw new InvalidOperationException();
			}

			return CreatedAtAction(nameof(Get), new { id }, monster);
		}

		/// <summary>
		/// モンスターを更新
		/// </summary>
		/// <param name="id">モンスターID</param>
		/// <param name="request">モンスター更新リクエスト</param>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public IActionResult Put(int id, [FromBody] MonsterUpdateRequest request) {
			if (!_monsters.ContainsKey(id)) {
				return BadRequest();
			}

			_monsters[id] = new Monster {
				Id = id,
				Name = request.Name,
			};

			return NoContent();
		}

		// todo:
		/*
		// DELETE api/<MonsterController>/5
		[HttpDelete("{id}")]
		public void Delete(int id) {
		}
		*/
	}
}
