namespace SampleLib.Linq;

public static class EnumerableHelper {
	// 引数が2つともnullか、SequenceEqualの場合、trueを返す
	public static bool BothNullOrSequenceEqual<TValue>(IEnumerable<TValue>? x, IEnumerable<TValue>? y) {
		// どちらもnullは等しい
		if (x is null && y is null) {
			return true;
		}

		// どちらかがnullの場合は等しくない
		if (x is null || y is null) {
			return false;
		}

		return x.SequenceEqual(y);
	}
}
