using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	public class MonsterFormModel : IValidatableObject {
		[Display(Name = "ID")]
		[Required(ErrorMessage = "{0}を入力してください")]
		public int Id { get; set; }

		[Display(Name = "名前")]
		[Required(ErrorMessage = "{0}を入力してください")]
		public string Name { get; set; }

		[Display(Name = "カテゴリ")]
		public MonsterCategory Category { get; set; }

		public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) {
			// ValidationContextはIServiceProviderを実装している
			// ModelMetadataProviderを取得
			var metadataProvider = validationContext.GetRequiredService<IModelMetadataProvider>();

			if (Category == MonsterCategory.None) {
				// プロパティのModelMetadataを取得
				var metadata = metadataProvider.GetMetadataForProperty(GetType(), nameof(Category));
				yield return new ValidationResult($"{metadata.DisplayName}を選択してください", new[] { nameof(Category) });
			}
		}
	}
}
