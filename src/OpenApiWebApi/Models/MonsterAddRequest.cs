using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Models;

/// <summary>
/// モンスター追加リクエスト
/// </summary>
public class MonsterAddRequest {
	/// <summary>
	/// モンスター名
	/// </summary>
	[Required]
	public string Name { get; init; } = "";
}
