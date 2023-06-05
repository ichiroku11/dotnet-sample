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
		// todo:
		// MinとMaxが正しいかチェックする

		if (value is not IEnumerable<string> values) {
			throw new InvalidOperationException();
		}

		/*
		// todo: 必要か？
		if (values.Any(value => value is null)) {
			throw new InvalidOperationException();
		}
		*/

		return values.All(value => value.Length >= Min && value.Length <= Max);
	}
}
