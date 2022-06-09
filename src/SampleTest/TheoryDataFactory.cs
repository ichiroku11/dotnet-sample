namespace SampleTest;

public static class TheoryDataFactory {
	public static TheoryData<TValue> CreateFrom<TValue>(IEnumerable<TValue> values) {
		var data = new TheoryData<TValue>();
		foreach (var value in values) {
			data.Add(value);
		}
		return data;
	}
}
