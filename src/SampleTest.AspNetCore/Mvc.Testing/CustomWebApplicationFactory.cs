using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace SampleTest.AspNetCore.Mvc.Testing;

// 同じモジュール内のコードに対して
// WebApplicationFactoryを使った統合テストを実施できるか試したがどうも無理そう
public class CustomWebApplicationFactory<TEntryPoint>
	: WebApplicationFactory<TEntryPoint>
	where TEntryPoint : class {

	protected override IHostBuilder? CreateHostBuilder() {
		//return base.CreateHostBuilder();
		return new HostBuilder();
	}
}
