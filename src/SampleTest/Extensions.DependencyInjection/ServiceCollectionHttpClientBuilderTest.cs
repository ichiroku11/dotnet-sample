using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionHttpClientBuilderTest {
	[Fact]
	public void AddHttpClient_HttpClientBuilderを取得できる() {
		// Arrange
		var services = new ServiceCollection();

		// Act
		var builder = services.AddHttpClient(name: "abc");

		// Assert
		Assert.Equal("abc", builder.Name);
		// 同じインスタンスではない？
		//Assert.Same(services, builder.Services);
	}
}
