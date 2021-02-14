using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TagHelperWebApp.Models {
	public static class ModelStateEntryExtensions {
		public static string GetRawValueAsString(this ModelStateEntry entry) {
			return entry.RawValue switch {
				string value => value,
				string[] values => string.Join(", ", values),
				_ => throw new NotImplementedException(),
			};
		}
	}
}
