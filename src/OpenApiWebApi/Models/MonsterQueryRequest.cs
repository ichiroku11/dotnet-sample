using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Models;

/// <summary>
/// モンスター問い合わせリクエスト
/// </summary>
public class MonsterQueryRequest {
	/// <summary>
	/// モンスター名
	/// </summary>
	public string Name { get; set; }
}
