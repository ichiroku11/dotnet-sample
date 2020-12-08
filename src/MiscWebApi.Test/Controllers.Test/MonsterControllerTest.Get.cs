using Microsoft.AspNetCore.Mvc;
using MiscWebApi.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MiscWebApi.Controllers.Test {
	public partial class MonsterControllerTest {
		[Fact]
		public async Task GetAsync_Ok() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, "/api/monster");

			// Act
			using var response = await SendAsync(request);
			var monsters = await DeserializeAsync<IList<Monster>>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(2, monsters.Count);
			Assert.Contains(monsters, monster => monster.Id == 1 && monster.Name == "スライム");
			Assert.Contains(monsters, monster => monster.Id == 2 && monster.Name == "ドラキー");
		}

		[Fact]
		public async Task GetByIdAsync_Ok() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, "/api/monster/1");

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(1, monster.Id);
			Assert.Equal("スライム", monster.Name);
		}

		[Fact]
		public async Task GetByIdAsync_NotFound() {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Get, "/api/monster/0");

			// Act
			using var response = await SendAsync(request);
			// エラーの場合は、ProblemDetails型（RFC7807）のJSONが返ってくる
			var problem = await DeserializeAsync<ProblemDetails>(response);

			// Assert
			Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
			Assert.NotNull(problem);
			Assert.Equal((int)HttpStatusCode.NotFound, problem.Status.Value);
		}

		// FromQuery属性：クエリ文字列から
		[Fact]
		public async Task GetQueryAsync_Ok() {
			// Arrange
			var url = $"/api/monster/query?id={_slime.Id}&name={_slime.Name}";
			using var request = new HttpRequestMessage(HttpMethod.Get, url);

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, monster.Id);
			Assert.Equal(_slime.Name, monster.Name);
		}

		// FromRoute属性：ルートパラメータからバインドできる
		[Fact]
		public async Task GetRouteAsync_Ok() {
			// Arrange
			var url = $"/api/monster/route/{_slime.Id}/{_slime.Name}";
			using var request = new HttpRequestMessage(HttpMethod.Get, url);

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, monster.Id);
			Assert.Equal(_slime.Name, monster.Name);
		}

		[Fact]
		public async Task GetRouteAsync_Query文字列はバインドされない() {
			// Arrange
			var url = $"/api/monster/route/2/ドラキー?id={_slime.Id}&name={_slime.Name}";
			using var request = new HttpRequestMessage(HttpMethod.Get, url);

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(2, monster.Id);
			Assert.Equal("ドラキー", monster.Name);
		}
	}
}
