using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiddlewareWebApp {
	public class SampleStartupFilter : IStartupFilter {
		public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next) {
			// 参考
			// https://docs.microsoft.com/en-us/aspnet/core/fundamentals/startup?view=aspnetcore-3.1
			return app => {
				// パイプラインの最初に処理するミドルウェアを登録する
				app.UseSample("First");

				next(app);

				// パイプラインの最後に処理するミドルウェアを登録するが、
				// EndpointRoutingMiddlewareで処理されるので基本的に呼ばれないはず
				app.UseSample("Last");
			};
		}
	}
}
