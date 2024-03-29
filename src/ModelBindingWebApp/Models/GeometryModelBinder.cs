using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Diagnostics.CodeAnalysis;

namespace ModelBindingWebApp.Models;

// GeometryModelをバインドする
public class GeometryModelBinder : IModelBinder {
	// サブクラスのバインダーとメタデータ
	private readonly Dictionary<Type, (IModelBinder binder, ModelMetadata metadata)> _binders = [];

	private bool TryGetSubclassBinder(
		Type type,
		[NotNullWhen(true)] out IModelBinder? binder,
		[NotNullWhen(true)] out ModelMetadata? metadata) {
		if (!_binders.ContainsKey(type)) {
			(binder, metadata) = (null, null);
			return false;
		}

		(binder, metadata) = _binders[type];
		return true;
	}

	public void AddSubclassBinder(Type type, IModelBinder binder, ModelMetadata metadata) {
		_binders.Add(type, (binder, metadata));
	}

	public async Task BindModelAsync(ModelBindingContext bindingContext) {
		// POSTされたデータかサブクラスを識別する値を取得
		var typePropertyName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, nameof(GeometryModel.GeometryType));
		var typeValue = bindingContext.ValueProvider.GetValue(typePropertyName).FirstValue;

		if (!Enum.TryParse<GeometryType>(typeValue, out var geometryType)) {
			bindingContext.Result = ModelBindingResult.Failed();
			return;
		}

		if (!TryGetSubclassBinder(geometryType.GetModelType(), out var newBinder, out var newMetadata)) {
			bindingContext.Result = ModelBindingResult.Failed();
			return;
		}

		// サブクラスとしてバインド
		var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
			actionContext: bindingContext.ActionContext,
			valueProvider: bindingContext.ValueProvider,
			metadata: newMetadata,
			bindingInfo: null,
			modelName: bindingContext.ModelName);
		await newBinder.BindModelAsync(newBindingContext);

		bindingContext.Result = newBindingContext.Result;
		if (newBindingContext.Result.IsModelSet) {
			// // Setting the ValidationState ensures properties on derived types are correctly
			bindingContext.ValidationState[newBindingContext.Result] = new ValidationStateEntry {
				Metadata = newMetadata,
			};
		}
	}
}
