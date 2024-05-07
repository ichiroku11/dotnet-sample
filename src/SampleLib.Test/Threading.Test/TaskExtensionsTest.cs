using Xunit;

namespace SampleLib.Threading.Test;

public class TaskExtensionsTest {
	[Fact]
	public async Task FireAndForget_試してみる() {
		// Arrange
		// Act
		// Assert
		var called = false;
		await Task.FromException(new ArgumentOutOfRangeException())
			.FireAndForget(exception => {
				Assert.IsType<ArgumentOutOfRangeException>(exception);
				called = true;
			});
		Assert.True(called);
	}
}
