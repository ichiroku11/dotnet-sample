using System.ComponentModel.DataAnnotations;

namespace SampleLib.ComponentModel.DataAnnotations;

/// <summary>
/// <paramref name="enumType"/>に変換できる文字列どうかを検証する属性
/// </summary>
/// <param name="enumType"></param>
/// <param name="excludes">除外する値</param>
public class AllowedEnumAttribute<TEnum>(params TEnum[] excludes) : ValidationAttribute
	where TEnum : struct, Enum {

	private readonly TEnum[] _excludes = excludes;

	public override bool IsValid(object? value) {
		// not nullのチェックはRequiredAttributeを使うことを想定し
		// nullの場合は有効
		if (value is null) {
			return true;
		}

		// 文字列ではない場合は無効
		if (value is not string text) {
			return false;
		}

		// 変換できない場合は無効
		if (!Enum.TryParse<TEnum>(text, true, out var parsed)) {
			return false;
		}

		// 除外に含まれる場合は無効
		if (_excludes.Contains(parsed)) {
			return false;
		}

		return true;
	}
}
