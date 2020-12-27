using Microsoft.AspNetCore.Mvc;
using OpenApiWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OpenApiWebApi.Controllers {
	[Route("api/[controller]")]
	[ApiController]
	public class MonsterController : ControllerBase {
		// GET: api/book
		[HttpGet]
		public IEnumerable<Monster> Get() {
			return new [] {
				new Monster {
					Id = 1,
					Name = "スライム"
				},
				new Monster {
					Id = 2,
					Name = "ドラキー"
				}
			};
		}

		// todo:
		/*
		// GET api/<BookController>/5
		[HttpGet("{id}")]
		public string Get(int id) {
			return "value";
		}

		// POST api/<BookController>
		[HttpPost]
		public void Post([FromBody] string value) {
		}

		// PUT api/<BookController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value) {
		}


		// DELETE api/<BookController>/5
		[HttpDelete("{id}")]
		public void Delete(int id) {
		}
		*/
	}
}
