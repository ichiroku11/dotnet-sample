namespace OpenApiWebApi.Models;

/// <summary>
/// モンスターカテゴリレスポンス
/// </summary>
public class MonsterCategoryResponse {
	/// <summary>
	/// カテゴリ
	/// </summary>
	public MonsterCategory Category { get; init; }

	/// <summary>
	/// カテゴリ名
	/// </summary>
	public string CategoryName { get; init; } = "";
}
