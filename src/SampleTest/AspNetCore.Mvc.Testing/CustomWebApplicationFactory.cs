using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace SampleTest.AspNetCore.Mvc.Testing;

// 同じモジュール内のコードに対して
// WebApplicationFactoryを使った統合テストを実施できるか試したがどうも無理そう
public class CustomWebApplicationFactory<TEntryPoint>
	: WebApplicationFactory<TEntryPoint>
	where TEntryPoint : class {

	protected override IWebHostBuilder CreateWebHostBuilder() {
		//return base.CreateWebHostBuilder();
		return new WebHostBuilder();
	}
}
