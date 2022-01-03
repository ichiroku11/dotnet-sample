namespace OpenApiWebApi.Models;

/// <summary>
/// モンスター問い合わせリクエスト
/// </summary>
public class MonsterQueryRequest {
	/// <summary>
	/// モンスター名
	/// </summary>
	public string Name { get; init; } = "";
}
