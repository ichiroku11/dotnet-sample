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

	public static TheoryData<DateTimeOffset?, DateTimeOffset> GetTheoryData_GetUtcNow() {
		var today = new DateTimeOffset(DateTime.UtcNow.Date);

		return new() {
			// 引数なしのコンストラクターで生成したインスタンスでGetUtcNowを呼び出した場合
			// 戻り値はデフォルト日時
			{ null, _default },
			// 日時を指定したコンストラクターで生成したインスタンスでGetUtcNowを呼び出した場合
			// 戻り値は指定した日時
			{ today, today }
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_GetUtcNow))]
	public void GetUtcNow_メソッドの戻り値を確認する(DateTimeOffset? start, DateTimeOffset expected) {
		// Arrange
		var timeProvider = start is null
			? new FakeTimeProvider()
			: new FakeTimeProvider(start.Value);

		// Act
		var actual = timeProvider.GetUtcNow();

		// Assert
		Assert.Equal(expected, actual);
	}

	public static TheoryData<DateTimeOffset?, long> GetTheoryData_GetTimestamp() {
		var today = new DateTimeOffset(DateTime.UtcNow.Date);

		return new() {
			// 引数なしのコンストラクターで生成したインスタンスでGetTimestampを呼び出した場合
			// 戻り値はデフォルト日時のTicks
			{ null, _default.Ticks },
			// 日時を指定したコンストラクターで生成したインスタンスでGetTimestampを呼び出した場合
			// 戻り値は指定した日時のTicks
			{ today, today.Ticks }
		};
	}

	[Theory]
	[MemberData(nameof(GetTheoryData_GetTimestamp))]
	public void GetTimestamp_メソッドの戻り値を確認する(DateTimeOffset? start, long expected) {
		// Arrange
		var timeProvider = start is null
			? new FakeTimeProvider()
			: new FakeTimeProvider(start.Value);

		// Act
		var actual = timeProvider.GetTimestamp();

		// Assert
		Assert.Equal(expected, actual);
	}
}
