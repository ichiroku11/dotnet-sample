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
		public enum PostContentType {
			FormUrlEncoded,
			JsonString,
			JsonStringTextPlain,
		}

		private static HttpContent GetPostContent(PostContentType contentType, Monster monster) {
			switch (contentType) {
				case PostContentType.FormUrlEncoded:
					// application/x-www-form-urlencoded
					var formValues = new Dictionary<string, string> {
						{ "id", monster.Id.ToString() },
						{ "name", monster.Name },
					};
					return new FormUrlEncodedContent(formValues);
				case PostContentType.JsonString:
				case PostContentType.JsonStringTextPlain:
					var content = GetJsonStringContent(monster);
					content.Headers.ContentType.MediaType
						= contentType == PostContentType.JsonString
							? "application/json"
							: "text/plain";
					return content;
			}

			throw new ArgumentOutOfRangeException(nameof(contentType));
		}

		#region FromXxx属性がないPOSTアクション
		// JSONをバインドできる
		[Theory]
		[InlineData(PostContentType.JsonString)]
		public async Task PostAsync_Ok(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, monster.Id);
			Assert.Equal(_slime.Name, monster.Name);
		}

		// Formデータをバインドできない
		// Content-Typeがtext/plainのJSONをバインドできない
		[Theory]
		[InlineData(PostContentType.FormUrlEncoded)]
		[InlineData(PostContentType.JsonStringTextPlain)]
		public async Task PostAsync_UnsupportedMediaType(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var problem = await DeserializeAsync<ProblemDetails>(response);

			// Assert
			Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
			Assert.NotNull(problem);
			Assert.Equal((int)HttpStatusCode.UnsupportedMediaType, problem.Status.Value);
		}

		// （Required属性の）バリエーションエラー
		[Fact]
		public async Task PostAsync_BadRequest() {
			// Arrange
			// NameのRequired属性でバリデーションエラー
			using var content = GetPostContent(PostContentType.JsonString, new Monster { Id = 1, Name = "" });
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var problem = await DeserializeAsync<ProblemDetails>(response);

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.NotNull(problem);
			Assert.Equal((int)HttpStatusCode.BadRequest, problem.Status.Value);
			// "errors"キーの値に、バリデーションエラーの内容が含まれている
			Assert.Contains("errors", problem.Extensions);
		}
		#endregion

		#region FromForm属性に対するPOSTアクション
		// Formデータをバインドできる
		[Theory]
		[InlineData(PostContentType.FormUrlEncoded)]
		public async Task PostFormAsync_Ok(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/form") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var monster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, monster.Id);
			Assert.Equal(_slime.Name, monster.Name);
		}

		// JSONをバインドできない（400が返ってくる）
		[Theory]
		[InlineData(PostContentType.JsonString)]
		public async Task PostFormAsync_BadRequest(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/form") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var problem = await DeserializeAsync<ProblemDetails>(response);

			// Assert
			Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
			Assert.NotNull(problem);
			Assert.Equal((int)HttpStatusCode.BadRequest, problem.Status.Value);
		}
		#endregion

		#region FromBody属性に対するPOSTアクション
		// Consumes属性がない場合
		// JSONをバインドできる
		[Theory]
		[InlineData(PostContentType.JsonString)]
		public async Task PostBodyAsync_Ok(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/body") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var responseMonster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, responseMonster.Id);
			Assert.Equal(_slime.Name, responseMonster.Name);
		}

		// Consumes属性がない場合
		// Formデータをバインドできない（415）
		// Content-Typeがtext/plainのJSONをバインドできない（415）
		// レスポンスはProblemDetailsのJSON
		[Theory]
		[InlineData(PostContentType.FormUrlEncoded)]
		[InlineData(PostContentType.JsonStringTextPlain)]
		public async Task PostBodyAsync_UnsupportedMediaType(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/body") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var problem = await DeserializeAsync<ProblemDetails>(response);

			// Assert
			Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
			Assert.NotNull(problem);
			Assert.Equal((int)HttpStatusCode.UnsupportedMediaType, problem.Status.Value);
		}

		// Consumes属性がある場合
		// JSONをバインドできる
		[Theory]
		[InlineData(PostContentType.JsonString)]
		public async Task PostBodyJsonAsync_Ok(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/body/json") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var responseMonster = await DeserializeAsync<Monster>(response);

			// Assert
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
			Assert.Equal(_slime.Id, responseMonster.Id);
			Assert.Equal(_slime.Name, responseMonster.Name);
		}

		// Consumes属性がある場合
		// Formデータをバインドできない（415）
		// Content-Typeがtext/plainのJSONをバインドできない（415）
		// レスポンスは空っぽ
		[Theory]
		[InlineData(PostContentType.FormUrlEncoded)]
		[InlineData(PostContentType.JsonStringTextPlain)]
		public async Task PostBodyJsonAsync_UnsupportedMediaType(PostContentType contentType) {
			// Arrange
			using var content = GetPostContent(contentType, _slime);
			using var request = new HttpRequestMessage(HttpMethod.Post, "/api/monster/body/json") {
				Content = content,
			};

			// Act
			using var response = await SendAsync(request);
			var responseText = await response.Content?.ReadAsStringAsync();

			// Assert
			Assert.Equal(HttpStatusCode.UnsupportedMediaType, response.StatusCode);
			Assert.NotNull(response.Content);
			Assert.NotNull(responseText);
			Assert.Equal(0, responseText.Length);
		}
		#endregion
	}
}
