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

	[Fact]
	public void Properties_引数を指定してインスタンスを生成した場合のプロパティを確認する() {
		// Arrange
		var today = new DateTimeOffset(DateTime.UtcNow.Date);

		// Act
		var provider = new FakeTimeProvider(today);

		// Assert
		Assert.Equal(provider.Start, today);
		Assert.Equal(TimeSpan.Zero, provider.AutoAdvanceAmount);
		Assert.Equal(TimeZoneInfo.Utc, provider.LocalTimeZone);
	}

	[Fact]
	public void GetUtcNow_引数なしのコンストラクターで生成したインスタンスで呼び出した場合の戻り値はデフォルト日時() {
		// Arrange
		var timeProvider = new FakeTimeProvider();

		// Act
		var actual = timeProvider.GetUtcNow();

		// Assert
		Assert.Equal(actual, _default);
	}

	[Fact]
	public void GetUtcNow_引数ありのコンストラクターで生成したインスタンスで呼び出した場合の戻り値は指定した日時() {
		// Arrange
		var today = new DateTimeOffset(DateTime.UtcNow.Date);
		var timeProvider = new FakeTimeProvider(today);

		// Act
		var actual = timeProvider.GetUtcNow();

		// Assert
		Assert.Equal(actual, today);
	}

	[Fact]
	public void GetTimestamp_引数なしのコンストラクターで生成したインスタンスで呼び出した場合の戻り値はデフォルト日時のTicks() {
		// Arrange
		var timeProvider = new FakeTimeProvider();

		// Act
		var actual = timeProvider.GetTimestamp();

		// Assert
		Assert.Equal(actual, _default.Ticks);
	}

	[Fact]
	public void GetTimestamp_引数ありのコンストラクターで生成したインスタンスで呼び出した場合の戻り値は指定した日時のTicks() {
		// Arrange
		var today = new DateTimeOffset(DateTime.UtcNow.Date);
		var timeProvider = new FakeTimeProvider(today);

		// Act
		var actual = timeProvider.GetTimestamp();

		// Assert
		Assert.Equal(actual, today.Ticks);
	}
}
