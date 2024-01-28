using System.Net;

namespace UdpConsoleApp;

class Program {
	// サーバの作成
	private static Server<string, string> Server(IPEndPoint endpoint) => new(endpoint);

	// クライアントの作成
	private static Client<string, string> Client(IPEndPoint endpoint) => new(endpoint);

	// 文字列を並びを反対にする
	private static string Reverse(string original) => new(original.Reverse().ToArray());

	static void Main(string[] args) {
		var endpoint = new IPEndPoint(IPAddress.Loopback, 54321);

		// サーバを実行
		var server = Task.Run(() => Server(endpoint).RunAsync(Reverse));

		// クライアントを実行
		Task.WaitAll(
			Client(endpoint).SendAsync("あいうえお"),
			Client(endpoint).SendAsync("かきくけこ"));

		// サーバのTaskをきれいに終了するにはどうしたらいいのか...
	}
}
