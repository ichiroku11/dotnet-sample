namespace OpenIdConnectWebApp.Line;

// LINEログイン
public static class LineDefaults {
	// 認証スキーマ
	public const string AuthenticationScheme = "Line";

	// メタデータ（OpenIDプロバイダーの情報）を取得するディスカバリーエンドポイント
	// 下記より
	// https://developers.line.biz/ja/docs/line-login/verify-id-token/#signature
	public const string MetadataAddress = "https://access.line.me/.well-known/openid-configuration";


}
