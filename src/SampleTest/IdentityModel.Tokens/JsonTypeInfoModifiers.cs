using System.Collections;
using System.Text.Json.Serialization.Metadata;

namespace SampleTest.IdentityModel.Tokens;

internal static class JsonTypeInfoModifiers {
	// コレクションのプロパティが空を返す場合、nullを返す
	internal static Action<JsonTypeInfo> ReturnNullIfCollectionEmpty = (JsonTypeInfo typeInfo) => {
		if (typeInfo.Kind is not JsonTypeInfoKind.Object) {
			return;
		}

		var properties = typeInfo.Properties.Where(property => property.PropertyType.IsAssignableTo(typeof(IEnumerable)));
		foreach (var property in properties) {
			var getter = property.Get;
			property.Get = (obj) => {
				if (getter?.Invoke(obj) is not IEnumerable values) {
					return null;
				}

				return values.Cast<object>().Any()
				? values
				: null;
			};
		}
	};
}
