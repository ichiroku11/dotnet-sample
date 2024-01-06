
using Polly;

namespace SampleTest.Polly;

public class PolicyBuilderOrTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	private class SampleException1 : Exception {
	}

	private class SampleException2 : Exception {
	}

	private class SampleException3 : Exception {
	}

	public static TheoryData<Exception, int> GetTheoryData_Or() {
		return new() {
			// Handleで指定したのでリトライされる
			{ new SampleException1(), 2 },

			// Orで指定したのでリトライされる
			{ new SampleException2(), 2 },

			// HnadleでもOrでも指定していないのでリトライされない
			{  new SampleException3(), 1 }
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_Or))]
	public void Or_指定した例外であればリトライされる(Exception exception, int expectedCount) {
		// Arrange
		var policy = Policy
			.Handle<SampleException1>()
			.Or<SampleException2>()
			.Retry();
		var actualCount = 0;

		// Act
		Assert.Throws(exception.GetType(), () => {
			policy.Execute(() => {
				actualCount++;
				_output.WriteLine($"Executed: {actualCount}");

				throw exception;
			});
		});

		// Assert
		Assert.Equal(expectedCount, actualCount);
	}
}
