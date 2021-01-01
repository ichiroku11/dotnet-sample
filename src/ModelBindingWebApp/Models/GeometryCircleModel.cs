using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	// 円
	public class GeometryCircleModel : GeometryModel {
		public override GeometryType GeometryType => GeometryType.Circle;

		// 中心座標と半径
		public int X { get; set; }
		public int Y { get; set; }
		public int R { get; set; }
	}
}
