namespace SampleTest.Xunit;

public class RecordTest {
	[Fact]
	public async Task ExceptionAsync_例外の発生を記録する() {
		var exception = await Record.ExceptionAsync(() => throw new InvalidOperationException());

		Assert.IsType<InvalidOperationException>(exception);
	}
}
