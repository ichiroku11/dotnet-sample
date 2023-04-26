using Microsoft.Extensions.DependencyInjection;

namespace SampleTest.Extensions.DependencyInjection;

public class ServiceCollectionTryAddEnumerableTest {
	private interface ISampleService {
	}

	private class SampleService1 : ISampleService {
	}

	private class SampleService2 : ISampleService {
	}
}
