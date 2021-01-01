using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	// ジオメトリ
	public abstract class GeometryModel {
		public abstract GeometryType GeometryType { get; }
	}
}
