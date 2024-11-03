using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;

namespace AzureStorageConsoleApp;

public class BlobSample(BlobServiceClient serviceClient, ILogger<BlobSample> logger) {
	private readonly BlobServiceClient _serviceClient = serviceClient;
	private readonly ILogger _logger = logger;

	public async Task RunAsync() {
		_logger.LogInformation(nameof(RunAsync));
		// 参考
		// https://docs.microsoft.com/ja-jp/azure/storage/blobs/storage-quickstart-blobs-dotnet

		var containerClient = _serviceClient.GetBlobContainerClient("sample");

		await containerClient.CreateIfNotExistsAsync();

		// Blobをアップロード
		await containerClient.UploadTextAsync("a.txt", "Aaa");
		await containerClient.UploadTextAsync("b.txt", "Bbb");

		// Blob一覧
		await foreach (var item in containerClient.GetBlobsAsync()) {
			var name = item.Name;
			_logger.LogInformation("{name}", name);
		}

		// Blobをダウンロード
		{
			var content = await containerClient.DownloadTextAsync("a.txt");
			_logger.LogInformation("{content}", content);
		}
		{
			var content = await containerClient.DownloadTextAsync("b.txt");
			_logger.LogInformation("{content}", content);
		}
	}
}
