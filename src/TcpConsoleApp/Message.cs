using System;
using System.Collections.Generic;
using System.Text;

namespace TcpConsoleApp;

/// <summary>
/// メッセージ
/// </summary>
public class Message {
	public int Id { get; set; }
	public string Content { get; set; }

	public override string ToString() {
		return $@"{{ {nameof(Id)} = {Id}, {nameof(Content)} = ""{Content}"" }}";
	}
}
