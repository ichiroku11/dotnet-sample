using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UdpConsoleApp;

/// <summary>
/// クライアント
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class Client<TRequest, TResponse> {
	// 送信先のエンドポイント
	private readonly IPEndPoint _endpoint;

	public Client(IPEndPoint endpoint) {
		_endpoint = endpoint;
	}

	/// <summary>
	/// クライアントを実行する
	/// リクエストを送信してレスポンスを受信する
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	public async Task<TResponse> SendAsync(TRequest request) {
		// UdpClientを作成するときにはエンドポイントを指定しない
		using var client = new UdpClient();

		// 1. リクエストを送信
		// （送信先のエンドポイントを指定して）
		Console.WriteLine($"Client send {nameof(request)}: {request}");
		var requestBytes = MessageHelper.ToByteArray(request);
		await client.SendAsync(requestBytes, requestBytes.Length, _endpoint);

		// 2. レスポンスを受信
		var result = await client.ReceiveAsync();

		// ここで受信した結果のリモートエンドポイントが送信先かをチェックした方がいい気がする

		var responseBytes = result.Buffer;
		var response = MessageHelper.FromByteArray<TResponse>(responseBytes);
		Console.WriteLine($"Client receive {nameof(response)}: {response}");

		return response;
	}
}
