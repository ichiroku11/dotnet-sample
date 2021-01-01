using Microsoft.AspNetCore.Mvc.Testing;
using ModelBindingWebApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ModelBindingWebApp.Controllers.Test {
	public class GeometryControllerTest : ControllerTestBase {
		private static readonly JsonSerializerOptions _jsonSerializerOptions
			= new JsonSerializerOptions {
				PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			};

		public GeometryControllerTest(
			ITestOutputHelper output,
			WebApplicationFactory<Startup> factory)
			: base(output, factory) {
		}

		public static IEnumerable<object[]> GetTestData() {
			yield return new object[] {
				new GeometryLineModel {
					X1 = 1,
					Y1 = 2,
					X2 = 3,
					Y2 = 4,
				},
			};

			yield return new object[] {
				new GeometryCircleModel {
					R = 1,
					X = 2,
					Y = 3,
				},
			};
		}

		// GeometryModelからPOSTデータを作成
		private static FormUrlEncodedContent GetContent(GeometryModel geometry) {
			var nameValues = geometry switch  {
				GeometryLineModel line => new Dictionary<string, string> {
					{ nameof(GeometryModel.GeometryType), line.GeometryType.ToString() },
					{ nameof(GeometryLineModel.X1), line.X1.ToString() },
					{ nameof(GeometryLineModel.Y1), line.Y1.ToString() },
					{ nameof(GeometryLineModel.X2), line.X2.ToString() },
					{ nameof(GeometryLineModel.Y2), line.Y2.ToString() },
				},
				GeometryCircleModel circle => new Dictionary<string, string> {
					{ nameof(GeometryModel.GeometryType), circle.GeometryType.ToString() },
					{ nameof(GeometryCircleModel.R), circle.R.ToString() },
					{ nameof(GeometryCircleModel.X), circle.X.ToString() },
					{ nameof(GeometryCircleModel.Y), circle.Y.ToString() },
				},
				_ => throw new ArgumentException(nameof(geometry)),
			};

			return new FormUrlEncodedContent(nameValues);
		}

		// サブクラスを考慮したGeometryModelの比較
		private class GeometryModelComparer : IEqualityComparer<GeometryModel> {
			public bool Equals([AllowNull] GeometryModel x, [AllowNull] GeometryModel y) {
				if (x.GetType() != y.GetType()) {
					return false;
				}

				if (x.GeometryType == GeometryType.Line && y.GeometryType == GeometryType.Line) {
					var lineX = x as GeometryLineModel;
					var lineY = y as GeometryLineModel;

					return lineX.X1 == lineY.X1
						&& lineX.Y1 == lineY.Y1
						&& lineX.X2 == lineY.X2
						&& lineX.Y2 == lineY.Y2;

				} else if (x.GeometryType == GeometryType.Circle && y.GeometryType == GeometryType.Circle) {
					var circleX = x as GeometryCircleModel;
					var circleY = x as GeometryCircleModel;

					return circleX.R == circleY.R
						&& circleX.X == circleY.X
						&& circleX.Y == circleY.Y;
				}

				throw new ArgumentException($"{nameof(x)}, {nameof(y)}");
			}

			public int GetHashCode([DisallowNull] GeometryModel obj) {
				return obj switch {
					GeometryLineModel line => HashCode.Combine(line.GeometryType, line.X1, line.Y1, line.X2, line.Y2),
					GeometryCircleModel circle => HashCode.Combine(circle.GeometryType, circle.R, circle.X, circle.Y),
					_ => throw new ArgumentException(nameof(obj)),
				};
			}
		}

		[Theory]
		[MemberData(nameof(GetTestData))]
		public async Task Save_サブクラスをバインドできる(GeometryModel expected) {
			// Arrange
			using var request = new HttpRequestMessage(HttpMethod.Post, "/geometry/save") {
				Content = GetContent(expected)
			};

			// Act
			using var response = await SendAsync(request);
			var content = await response.Content.ReadAsStringAsync();
			var actual = JsonSerializer.Deserialize(content, expected.GetType(), _jsonSerializerOptions) as GeometryModel;

			// Assert
			Assert.IsType(expected.GetType(), actual);
			Assert.Equal(expected, actual, new GeometryModelComparer());
			Assert.Equal(HttpStatusCode.OK, response.StatusCode);
		}
	}
}
