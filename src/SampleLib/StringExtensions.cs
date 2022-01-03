using System.Text.RegularExpressions;

namespace SampleLib;

public static class StringExtensions {
	// ケバブケースに変換できる文字列の正規表現
	// 半角英数字が1文字以上
	private static readonly Regex _regexToKebab = new("^[0-9a-zA-Z]+$");

	// enumの定義をケバブケースにするイメージ
	/// <summary>
	/// パスカルケース・キャメルケースをケバブケースに変換する
	/// </summary>
	/// <param name="original"></param>
	/// <returns></returns>
	public static string ToKebabCase(this string original) {
		if (!_regexToKebab.IsMatch(original)) {
			throw new ArgumentException("", nameof(original));
		}

		var values = new List<char>() {
				char.ToLower(original[0])
			};

		// 2文字目以降の大文字 => "-" + 小文字
		foreach (var ch in original.Skip(1)) {
			if (char.IsUpper(ch)) {
				values.Add('-');
				values.Add(char.ToLower(ch));
			} else {
				values.Add(ch);
			}
		}

		return string.Concat(values);
	}
}
