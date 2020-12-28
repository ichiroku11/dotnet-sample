using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Models {
	// summaryがOpenAPIに出力される
	/// <summary>
	/// モンスター
	/// </summary>
	public class Monster {
		/// <summary>
		/// ID
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// 名前
		/// </summary>
		public string Name { get; set; }
	}
}
