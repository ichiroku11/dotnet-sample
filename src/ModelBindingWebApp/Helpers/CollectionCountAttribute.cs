using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Helpers;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class CollectionCountAttribute(int max) : ValidationAttribute {
	public int Min { get; init; }

	public int Max { get; } = max;

	public override bool IsValid(object? value) {
		if (Max < 0) {
			throw new InvalidOperationException();
		}
		if (Max < Min) {
			throw new InvalidOperationException();
		}

		// nullについては別のバリデーションとする
		if (value is null) {
			return true;
		}
		
		if (value is not ICollection values) {
			throw new InvalidOperationException();
		}

		return values.Count >= Min && values.Count <= Max;
	}
}
