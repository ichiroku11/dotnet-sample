using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	// バインドする型がGeometryModelならGeometryModelBinderを提供する
	public class GeometryModelBinderProvider : IModelBinderProvider {
		public IModelBinder GetBinder(ModelBinderProviderContext context) {
			if (context.Metadata.ModelType != typeof(GeometryModel)) {
				return null;
			}

			var geometryBinder = new GeometryModelBinder();

			var subclassTypes = new[] {
				typeof(GeometryLineModel),
				typeof(GeometryCircleModel),
			};
			foreach (var subclassType in subclassTypes) {
				var subclassMetadata = context.MetadataProvider.GetMetadataForType(subclassType);
				var subclassBinder = context.CreateBinder(subclassMetadata);

				geometryBinder.AddSubclassBinder(subclassType, subclassBinder, subclassMetadata);
			}

			return geometryBinder;
		}
	}
}
