namespace TcpConsoleApp;

/// <summary>
/// メッセージ
/// </summary>
public class Message {
	public int Id { get; set; }
	public string Content { get; set; } = "";

	public override string ToString() {
		return $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
	}
}
