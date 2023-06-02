using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Helpers;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class CollectionLengthAttribute : ValidationAttribute {
	public CollectionLengthAttribute(int maxLength) {
		MaxLength = maxLength;
	}

	public int MinLength { get; init; }

	public int MaxLength { get; }

	public override bool IsValid(object? value) {
		// todo:
		// MinLengthとMaxLengthが正しいかチェックする
		// IEnumerableも対象とするか

		if (value is not ICollection values) {
			throw new InvalidOperationException();
		}

		return values.Count >= MinLength && values.Count <= MaxLength;
	}
}
