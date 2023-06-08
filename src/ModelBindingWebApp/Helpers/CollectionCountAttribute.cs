using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Helpers;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class CollectionCountAttribute : ValidationAttribute {
	public CollectionCountAttribute(int max) {
		Max = max;
	}

	public int Min { get; init; }

	public int Max { get; }

	public override bool IsValid(object? value) {
		if (Max < 0) {
			throw new InvalidOperationException();
		}
		if (Max < Min) {
			throw new InvalidOperationException();
		}

		if (value is not ICollection values) {
			throw new InvalidOperationException();
		}

		return values.Count >= Min && values.Count <= Max;
	}
}
