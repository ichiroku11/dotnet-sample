namespace NamedPipeConsoleApp;

/// <summary>
/// メッセージ
/// </summary>
public class Message {
	public int Id { get; set; }
	public string Content { get; set; } = "";

	public override string ToString()
		=> $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
}
