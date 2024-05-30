using Microsoft.Extensions.Time.Testing;

namespace SampleTest.Extensions.Time.Testing;

public class FakeTimeProviderTest {
	private static readonly DateTimeOffset _default = new(2000, 1, 1, 0, 0, 0, TimeSpan.Zero);

	[Fact]
	public void Properties_引数なしでインスタンスを生成した場合のプロパティを確認する() {
		// Arrange
		// Act
		var provider = new FakeTimeProvider();

		// Assert
		// デフォルトは、UTCの2000/01/01 00:00:00
		Assert.Equal(provider.Start, _default);
		Assert.Equal(TimeSpan.Zero, provider.AutoAdvanceAmount);
		Assert.Equal(TimeZoneInfo.Utc, provider.LocalTimeZone);
	}
}
