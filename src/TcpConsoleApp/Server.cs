using System.Net;
using System.Net.Sockets;

namespace TcpConsoleApp;

/// <summary>
/// サーバ
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class Server<TRequest, TResponse> {
	// 接続を待つエンドポイント
	private readonly IPEndPoint _endpoint;

	// リクエストからレスポンスを作成する処理
	private readonly Func<TRequest, TResponse> _processor;

	// TCPリスナー
	private readonly TcpListener _listener;

	public Server(IPEndPoint endpoint, Func<TRequest, TResponse> processor) {
		_endpoint = endpoint;
		_processor = processor;
		_listener = new TcpListener(_endpoint);
	}

	// クライアントからリクエストを受信してレスポンスを送信する
	private async Task ReceiveAsync(TcpClient client) {
		using (client) {
			using var stream = client.GetStream();

			// 3. クライアントからリクエストを受信する
			var request = await stream.ReadFromJsonAsync<TRequest>();

			// 4. リクエストを処理してレスポンスを作る
			var response = _processor(request);

			// 5. クライアントにレスポンスを送信する
			await stream.WriteAsJsonAsync(response);
		}
	}

	/// <summary>
	/// 接続を待つ
	/// </summary>
	/// <returns></returns>
	public async Task RunAsync() {
		Console.WriteLine($"Server listen:");
		// 1. クライアントからの接続を待つ
		_listener.Start();

		while (true) {
			// 2. クライアントからの接続を受け入れる
			var client = await _listener.AcceptTcpClientAsync();
			Console.WriteLine($"Server accepted:");

			var task = ReceiveAsync(client);

			// Taskの管理やエラー処理は省略
		}
	}

	/// <summary>
	/// 終了する
	/// </summary>
	public void Close() {
		_listener.Stop();
	}
}
