using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.AspNetCore {
	// 参考
	// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/test/Routing/UrlHelperTest.cs
	// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/test/Routing/UrlHelperTestBase.cs
	public class UrlHelperTest {
		private class PassThroughRouter : IRouter {
			public VirtualPathData GetVirtualPath(VirtualPathContext context) => null;

			public Task RouteAsync(RouteContext routeContext) {
				routeContext.Handler = httpContext => Task.CompletedTask;
				return Task.CompletedTask;
			}
		}

		// IServiceProvider（サービス一覧）を生成
		private static IServiceProvider CreateServiceProvider() {
			var services = new ServiceCollection();

			services
				.AddOptions()
				.AddLogging()
				.AddRouting(options => {
					options.LowercaseQueryStrings = true;
					options.LowercaseUrls = true;
				})
				.AddSingleton(UrlEncoder.Default);

			return services.BuildServiceProvider();
		}

		// HttpContextを生成
		private static HttpContext CreateHttpContext(
			IServiceProvider services,
			string scheme,
			string host,
			string app) {
			var context = new DefaultHttpContext {
				RequestServices = services
			};

			var request = context.Request;
			request.Scheme = scheme;
			request.Host = new HostString(host);
			request.PathBase = new PathString(app);

			return context;
		}

		// ActionContextを生成
		private static ActionContext CreateActionContext(HttpContext httpContext, RouteData routeData = default)
			=> new ActionContext(httpContext, routeData ?? new RouteData(), new ActionDescriptor());

		// IRouteBuilderを生成
		private static IRouteBuilder CreateRouteBuilder(IServiceProvider services) {
			var app = new Mock<IApplicationBuilder>();

			app.SetupGet(a => a.ApplicationServices)
				.Returns(services);

			return new RouteBuilder(app.Object) {
				DefaultHandler = new PassThroughRouter(),
			};
		}

		// IRouterを生成
		private static IRouter CreateRouter(IServiceProvider services) {
			var routeBuilder = CreateRouteBuilder(services);
			// デフォルトのルートを追加
			routeBuilder.MapRoute(
				name: "default",
				template: "{controller}/{action}/{id?}",
				new { controller = "default", action = "index" });
			return routeBuilder.Build();
		}

		// UrlHelperを生成
		private static UrlHelper CreateUrlHelper(string scheme, string host, string app) {
			var services = CreateServiceProvider();
			var httpContext = CreateHttpContext(services, scheme, host, app);
			var actionContext = CreateActionContext(httpContext);
			actionContext.RouteData.Routers.Add(CreateRouter(services));

			return new UrlHelper(actionContext);
		}

		public static IEnumerable<object[]> GetTestDataForActionLink() {
			yield return new[] {
				"example.jp", "", null, null, null, "https://example.jp/"
			};
			// appあり
			yield return new[] {
				"example.jp", "/app", null, null, null, "https://example.jp/app"
			};
			// actionあり
			yield return new[] {
				"example.jp", "/app", "x", null, null, "https://example.jp/app/default/x"
			};
			// action/controllerあり
			yield return new[] {
				"example.jp", "/app", "x", "y", null, "https://example.jp/app/y/x"
			};
			// パラメータあり（ルートに含まれる）
			yield return new object[] {
				"example.jp", "/app", "x", "y", new { id = 1 }, "https://example.jp/app/y/x/1"
			};
			// パラメータあり（クエリ文字列）
			yield return new object[] {
				"example.jp", "/app", "x", "y", new { value = "abc" }, "https://example.jp/app/y/x?value=abc"
			};
		}

		[Theory]
		[MemberData(nameof(GetTestDataForActionLink))]
		public void ActionLink_絶対URLを生成できる(
			string host, string app,
			string action, string contoller, object values,
			string expected) {
			// Arrange
			var urlHelper = CreateUrlHelper("https", host, app);

			// Act
			var actual = urlHelper.ActionLink(action, contoller, values);

			// Assert
			Assert.Equal(expected: expected, actual);
		}
	}
}
