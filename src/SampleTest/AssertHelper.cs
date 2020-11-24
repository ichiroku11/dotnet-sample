using System;
using Xunit;

namespace SampleTest {
	public static class AssertHelper {
		public static void Fail(string message = null) {
			Assert.True(false, message ?? "Never be called.");
		}
	}
}
