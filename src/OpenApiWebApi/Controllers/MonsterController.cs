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

namespace OpenApiWebApi.Controllers {
	// コントローラにApiConventionType属性を指定し、
	// 各メソッドでProducesResponseType属性が存在しない場合に、
	// typeof(DefaultApiConventions)の型で指定された規約が適用される様子
	// https://docs.microsoft.com/ja-jp/aspnet/core/web-api/advanced/conventions?view=aspnetcore-5.0
	//[ApiConventionType(typeof(DefaultApiConventions))]

	// クラスののsummaryがOpenAPIに出力されない？
	/// <summary>
	/// モンスターコントローラ
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	[ApiConventionType(typeof(ApiConventions))]
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
		/// <param name="request">モンスター問い合わせリクエスト</param>
		/// <returns></returns>
		[HttpGet]
		// ApiConventionType属性による規約が適用されないのでProducesResponseType属性を指定する
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		[ProducesDefaultResponseType]
		public IEnumerable<Monster> Get([FromQuery] MonsterQueryRequest request) {
			var monsters = _monsters
				.OrderBy(entry => entry.Key)
				.Select(entry => entry.Value);

			monsters = string.IsNullOrEmpty(request.Name)
				? monsters
				: monsters.Where(monster => monster.Name.Contains(request.Name));

			return monsters;
		}

		/// <summary>
		/// モンスターIDでモンスターを取得
		/// </summary>
		/// <param name="id">モンスターID</param>
		/// <returns></returns>
		[HttpGet("{id}")]
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
		public ActionResult<Monster> Post([FromBody] MonsterAddRequest request) {
			var id = Interlocked.Increment(ref _maxId);
			var monster = new Monster {
				Id = id,
				Name = request.Name,
			};

			if (!_monsters.TryAdd(id, monster)) {
				// 追加に失敗することはありえないはず
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
		public IActionResult Put(int id, [FromBody] MonsterUpdateRequest request) {
			if (!_monsters.ContainsKey(id)) {
				return NotFound();
			}

			_monsters[id] = new Monster {
				Id = id,
				Name = request.Name,
			};

			return NoContent();
		}

		/// <summary>
		/// モンスターを削除
		/// </summary>
		/// <param name="id">モンスターID</param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		public IActionResult Delete(int id) {
			if (!_monsters.ContainsKey(id)) {
				return NotFound();
			}

			if (!_monsters.TryRemove(id, out _)) {
				// 2つのリクエストから同時に削除された場合に失敗するはずだが、エラーとするかは仕様次第か
				throw new InvalidOperationException();
			}

			return NoContent();
		}
	}
}
