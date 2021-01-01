using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelBindingWebApp.Models {
	// 線分
	public class GeometryLineModel : GeometryModel {
		public override GeometryType GeometryType => GeometryType.Line;

		// 始点座標と終点座標
		// 本当ならdoubleだろうけど今回は手抜きでint
		public int X1 { get; set; }
		public int Y1 { get; set; }
		public int X2 { get; set; }
		public int Y2 { get; set; }
	}
}
