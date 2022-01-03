using System.ComponentModel.DataAnnotations;

namespace OpenApiWebApi.Models;

/// <summary>
/// モンスター更新リクエスト
/// </summary>
public class MonsterUpdateRequest {
	/// <summary>
	/// モンスター名
	/// </summary>
	[Required]
	public string Name { get; init; } = "";
}
