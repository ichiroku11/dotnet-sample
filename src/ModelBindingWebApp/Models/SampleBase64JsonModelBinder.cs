using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ModelBindingWebApp.Models;

public class SampleBase64JsonModelBinder : IModelBinder {
	public Task BindModelAsync(ModelBindingContext bindingContext) {
		// https://docs.microsoft.com/ja-jp/aspnet/core/mvc/advanced/custom-model-binding?view=aspnetcore-6.0

		var modelName = bindingContext.ModelName;
		var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
		if (valueProviderResult == ValueProviderResult.None) {
			return Task.CompletedTask;
		}

		bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

		var value = valueProviderResult.FirstValue;

		// todo:

		return Task.CompletedTask;
	}
}
