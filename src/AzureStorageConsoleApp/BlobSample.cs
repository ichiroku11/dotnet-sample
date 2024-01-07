using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AzureStorageConsoleApp;

public class BlobSample(IConfiguration config, ILogger<BlobSample> logger) {
	private readonly string _connectionString = config.GetConnectionString("Storage") ?? throw new InvalidOperationException();
	private readonly ILogger _logger = logger;

	public async Task RunAsync() {
		_logger.LogInformation(nameof(RunAsync));
		// 参考
		// https://docs.microsoft.com/ja-jp/azure/storage/blobs/storage-quickstart-blobs-dotnet

		var serviceClient = new BlobServiceClient(_connectionString);

		var containerClient = serviceClient.GetBlobContainerClient("sample");

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
