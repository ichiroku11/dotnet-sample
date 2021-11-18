using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpConsoleApp;

/// <summary>
/// クライアント
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class Client<TRequest, TResponse> {
	// 接続先のエンドポイント
	private readonly IPEndPoint _endpoint;

	public Client(IPEndPoint endpoint) {
		_endpoint = endpoint;
	}

	/// <summary>
	/// サーバにリクエストを送信してレスポンスを受信する
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public async Task<TResponse> SendAsync(TRequest request) {
		using var client = new TcpClient();
		// 1. サーバに接続
		await client.ConnectAsync(_endpoint.Address, _endpoint.Port);
		Console.WriteLine($"Client connected:");

		using var stream = client.GetStream();
		// 2. サーバにリクエストを送信する
		Console.WriteLine($"Client sent: {request}");
		await stream.WriteAsJsonAsync(request);

		// 3. サーバからレスポンスを受信する
		var response = await stream.ReadFromJsonAsync<TResponse>();
		Console.WriteLine($"Client received: {response}");

		return response;
	}
}
