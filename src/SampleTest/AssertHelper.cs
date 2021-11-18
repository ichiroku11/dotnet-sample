using System;
using Xunit;

namespace SampleTest;

/// <summary>
/// 
/// </summary>
public static class AssertHelper {
	/// <summary>
	/// 
	/// </summary>
	/// <param name="message"></param>
	public static void Fail(string message = null) {
		Assert.True(false, message ?? "Never be called.");
	}
}
