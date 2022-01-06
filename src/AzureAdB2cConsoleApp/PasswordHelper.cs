namespace AzureAdB2cConsoleApp;

public static class PasswordHelper {
	private const string _lowers = "abcdefghijklmnopqrstuvwxyz";
	private const string _uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
	private const string _numbers = "0123456789";
	//private const string _symbols = "";

	// パスワード生成
	public static string Generate(int lower, int upper, int number) {
		var random = new Random();

		var lowers = Enumerable.Range(0, lower).Select(_ => _lowers[random.Next(_lowers.Length)]);
		var uppers = Enumerable.Range(0, upper).Select(_ => _uppers[random.Next(_uppers.Length)]);
		var numbers = Enumerable.Range(0, number).Select(_ => _numbers[random.Next(_numbers.Length)]);

		var password = lowers
			.Concat(uppers)
			.Concat(numbers)
			// シャッフル
			.OrderBy(_ => random.Next(lower + upper + number))
			.ToArray();

		return new string(password);
	}
}
