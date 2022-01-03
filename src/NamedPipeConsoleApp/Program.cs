using System.IO.Pipes;

namespace NamedPipeConsoleApp;

class Program {
	// パイプの名前
	private static readonly string _pipeName = "testpipe";

	// クライアント
	private static async Task<Message> Client(int clientId, Message request) {
		using var stream = new NamedPipeClientStream(_pipeName);

		// サーバに接続
		Console.WriteLine($"Client#{clientId} connecting");
		await stream.ConnectAsync();
		Console.WriteLine($"Client#{clientId} connected");

		// サーバにリクエストを送信する
		Console.WriteLine($"Client#{clientId} {nameof(request)}: {request}");
		await stream.WriteAsJsonAsync(request);

		// サーバからレスポンスを受信する
		var response = await stream.ReadFromJsonAsync<Message>();
		Console.WriteLine($"Client#{clientId} {nameof(response)}: {response}");

		return response;
	}

	// サーバ
	private static async Task Server(int serverId, Func<Message, Message> processor) {
		while (true) {
			//using var stream = new NamedPipeServerStream(_pipeName);
			// サーバインスタンスを2に
			using var stream = new NamedPipeServerStream(_pipeName, PipeDirection.InOut, 2);
			// クライアントからの接続を待つ
			Console.WriteLine($"Server#{serverId} waiting");
			await stream.WaitForConnectionAsync();
			Console.WriteLine($"Server#{serverId} connected");

			// クライアントからリクエストを受信する
			var request = await stream.ReadFromJsonAsync<Message>();
			Console.WriteLine($"Server#{serverId} {nameof(request)}: {request}");

			// リクエストを処理してレスポンスを作る
			var response = processor(request);

			// クライアントにレスポンスを送信する
			Console.WriteLine($"Server#{serverId} {nameof(response)}: {response}");
			await stream.WriteAsJsonAsync(response);
		}
	}

	// サーバでの処理
	// Contentの文字列を逆順にする処理
	private static Message ServerProcess(Message message) {
		return new Message {
			Id = message.Id,
			Content = new string(message.Content.Reverse().ToArray()),
		};
	}

	static void Main(string[] args) {
		Task.WaitAll(new[] {
				// クライアント
				Client(1, new Message { Id = 10, Content = "あいうえお", }),
				Client(2, new Message { Id = 20, Content = "かきくけこ", }),
				// サーバ
				Server(1, ServerProcess),
				Server(2, ServerProcess),
			});
	}
}
