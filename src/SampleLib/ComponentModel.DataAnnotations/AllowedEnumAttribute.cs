using System.ComponentModel.DataAnnotations;

namespace SampleLib.ComponentModel.DataAnnotations;

/// <summary>
/// <paramref name="enumType"/>に変換できる文字列かどうかを検証する属性
/// </summary>
/// <param name="enumType"></param>
public class AllowedEnumAttribute(Type enumType) : ValidationAttribute {
	private readonly Type _enumType = enumType;

	public override bool IsValid(object? value) {
		if (!_enumType.IsAssignableTo(typeof(Enum))) {
			throw new InvalidOperationException();
		}

		// not nullのチェックはRequiredAttributeを使うことを想定し
		// nullの場合は、trueを返す
		if (value is null) {
			return true;
		}

		// 文字列ではない場合は、falseを返す
		if (value is not string text) {
			return false;
		}

		return Enum.TryParse(_enumType, text, true, out var _);
	}
}
