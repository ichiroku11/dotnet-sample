using System.ComponentModel.DataAnnotations;

namespace ModelBindingWebApp.Helpers;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class AllStringLengthAttribute : ValidationAttribute {
	public AllStringLengthAttribute(int max) {
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

		// nullは別のバリデーションで
		if (value is null) {
			return true;
		}

		if (value is not IEnumerable<string> values) {
			throw new InvalidOperationException();
		}

		return values.All(value => value.Length >= Min && value.Length <= Max);
	}
}
