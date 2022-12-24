namespace SampleLib;

public static class DateTimeExtensions {
	// ミリ秒を切り捨てる（秒で切り捨てる）
	// https://stackoverflow.com/questions/1004698/how-to-truncate-milliseconds-off-of-a-net-datetime
	public static DateTime TruncateMilliseconds(this DateTime dateTime)
		=> dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
}
