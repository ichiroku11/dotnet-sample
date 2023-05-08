using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionOptionsTest {
	private readonly ITestOutputHelper _output;

	public ServiceCollectionOptionsTest(ITestOutputHelper output) {
		_output = output;
	}
}
