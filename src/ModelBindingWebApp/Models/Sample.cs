using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	public class Sample : IEquatable<Sample> {
		[Required]
		public int Id { get; set; }
		[Required]
		public string Name { get; set; }

		public bool Equals([AllowNull] Sample other) {
			return Id.Equals(Id) && Name.Equals(other.Name);
		}
	}
}
