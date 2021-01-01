using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	// ジオメトリタイプ
	public enum GeometryType {
		Unknown = 0,
		// 線分
		Line = 1,
		// 円
		Circle = 2,
	}

	public static class GeometryTypeExtensions {
		// GeometryTypeからモデルの型を取得
		public static Type GetModelType(this GeometryType geometryType) {
			return geometryType switch {
				GeometryType.Line => typeof(GeometryLineModel),
				GeometryType.Circle => typeof(GeometryCircleModel),
				_ => throw new NotImplementedException()
			};
		}
	}
}
