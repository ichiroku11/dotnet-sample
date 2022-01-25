using Xunit;

namespace SampleTest;

/// <summary>
/// <see cref="CollectionAttribute"/>で使用する名前
/// </summary>
public static class CollectionNames {
	/// <summary>
	/// dbo.Blogテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreBlog = "EFCore_dbo.Blog";

	/// <summary>
	/// dbo.Sampleテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreSample = "EFCore_dbo.Sample";

	/// <summary>
	/// dbo.Monsterテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreMonster = "EFCore_dbo.Monster";
}
