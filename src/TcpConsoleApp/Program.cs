using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace TcpConsoleApp {
	class Program {
		static void Main(string[] args) {
			// サーバが接続を待つエンドポイント
			// であり
			// クライアントが接続するサーバのエンドポイント
			var endpoint = new IPEndPoint(IPAddress.Loopback, 54321);

			// サーバ
			var server = new Server<Message, Message>(
				endpoint,
				// リクエストからレスポンスを作る処理
				request => new Message {
					Id = request.Id,
					// メッセージの文字列を逆順にする
					Content = new string(request.Content.Reverse().ToArray()),
				});
			// 接続を待機
			var task = Task.Run(() => server.RunAsync());

			// クライアント
			Task.WaitAll(
				// リクエストを送信してレスポンスを受信
				new Client<Message, Message>(endpoint).SendAsync(new Message { Id = 10, Content = "あいうえお" }),
				new Client<Message, Message>(endpoint).SendAsync(new Message { Id = 20, Content = "かきくけこ" }),
				new Client<Message, Message>(endpoint).SendAsync(new Message { Id = 30, Content = "さしすせそ" })
			);

			// サーバを終了
			server.Close();
			// サーバの終了処理、Taskの管理、エラー処理あたりが微妙
		}
	}
}
