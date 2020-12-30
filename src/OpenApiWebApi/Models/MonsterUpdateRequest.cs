using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Models {
	/// <summary>
	/// モンスター更新リクエスト
	/// </summary>
	public class MonsterUpdateRequest {
		/// <summary>
		/// モンスター名
		/// </summary>
		[Required]
		public string Name { get; set; }
	}
}
