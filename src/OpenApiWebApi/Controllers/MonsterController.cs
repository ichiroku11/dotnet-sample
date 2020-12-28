using Microsoft.AspNetCore.Mvc;
using OpenApiWebApi.Models;
using System;
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
	public class MonsterController : ControllerBase {

		private static readonly Dictionary<int, Monster> _monsters
			= new[] {
				new Monster {
					Id = 1,
					Name = "スライム"
				},
				new Monster {
					Id = 2,
					Name = "ドラキー"
				}
			}.ToDictionary(monster => monster.Id);

		// アクションメソッドのsummaryがOpenAPIに出力される
		// GET: api/monster
		/// <summary>
		/// <see cref="Monster"/>一覧を取得
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		public IEnumerable<Monster> Get() {
			return _monsters.OrderBy(entry => entry.Key).Select(entry => entry.Value);
		}

		// todo:
		/*
		// GET api/<MonsterController>/5
		[HttpGet("{id}")]
		public string Get(int id) {
		}

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
