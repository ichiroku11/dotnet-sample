namespace OpenApiWebApi.Models;

// summaryがOpenAPIに出力される
/// <summary>
/// モンスター
/// </summary>
public class Monster {
	/// <summary>
	/// ID
	/// </summary>
	public int Id { get; init; }
	/// <summary>
	/// 名前
	/// </summary>
	public string Name { get; init; } = "";
}
