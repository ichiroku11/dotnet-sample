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
	/// dbo.Mailテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreMail = "EFCore_dbo.Mail";

	/// <summary>
	/// dbo.Outer、dbo.Innerテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreOuterInner = "EFCore_dbo.Outer_dbo.Inner";

	/// <summary>
	/// dbo.Sampleテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreSample = "EFCore_dbo.Sample";

	/// <summary>
	/// dbo.SQ_Sampleシーケンスを使用するコレクション名
	/// </summary>
	public const string EfCoreSampleSequence = "EFCore_dbo.SQ_Sample";

	/// <summary>
	/// dbo.Monsterテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreMonster = "EFCore_dbo.Monster";

	/// <summary>
	/// dbo.TodoItemテーブルを使用するコレクション名
	/// </summary>
	public const string EfCoreTodoItem = "EFCore_dbo.TodoItem";
}
