using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text;
using System.Text.Json;

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

		// todo:
		var base64 = valueProviderResult.FirstValue ?? "";
		var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
		var model = JsonSerializer.Deserialize<Sample>(json);
		if (model is null) {
			bindingContext.Result = ModelBindingResult.Failed();
			return Task.CompletedTask;
		}

		bindingContext.Result = ModelBindingResult.Success(model);

		return Task.CompletedTask;
	}
}
