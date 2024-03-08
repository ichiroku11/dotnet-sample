using System.ComponentModel.DataAnnotations;

namespace SampleLib.ComponentModel.DataAnnotations;

/// <summary>
/// <paramref name="enumType"/>に変換できる文字列どうかを検証する属性
/// </summary>
/// <param name="enumType"></param>
public class AllowedEnumAttribute<TEnum> : ValidationAttribute
	where TEnum : Enum {

	public override bool IsValid(object? value) {
		// not nullのチェックはRequiredAttributeを使うことを想定し
		// nullの場合は、trueを返す
		if (value is null) {
			return true;
		}

		// 文字列ではない場合は、falseを返す
		if (value is not string text) {
			return false;
		}

		return Enum.TryParse(typeof(TEnum), text, true, out var _);
	}
}
