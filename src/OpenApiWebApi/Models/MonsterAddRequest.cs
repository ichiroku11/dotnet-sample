using System.ComponentModel.DataAnnotations;

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
