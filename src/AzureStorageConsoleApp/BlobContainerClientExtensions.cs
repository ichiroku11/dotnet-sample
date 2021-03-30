using Azure.Storage.Blobs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageConsoleApp {
	public static class BlobContainerClientExtensions {
		public static async Task<bool> UploadTextAsync(
			this BlobContainerClient containerClient, string blobName, string blobContent) {
			var bytes = Encoding.UTF8.GetBytes(blobContent);
			var hash = MD5.Create().ComputeHash(bytes);
			using var stream = new MemoryStream(bytes);

			var blobClient = containerClient.GetBlobClient(blobName);

			// Response<bool>
			var exists = await blobClient.ExistsAsync();
			if (exists) {
				return false;
			}

			// 上書き=falseでアップロードすると、ファイルが存在した場合にRequestFailedException
			// Response<BlobContentInfo>
			var contentInfo = await blobClient.UploadAsync(stream);

			// データのMD5ハッシュの値を取得できるっぽい
			return hash.SequenceEqual(contentInfo.Value.ContentHash);
		}

		public static async Task<string> DownloadTextAsync(
			this BlobContainerClient containerClient, string blobName) {
			var blobClient = containerClient.GetBlobClient(blobName);

			// Response<BlobDownloadInfo>
			var downloadInfo = await blobClient.DownloadAsync();

			using var reader = new StreamReader(downloadInfo.Value.Content, Encoding.UTF8);
			return await reader.ReadToEndAsync();
		}
	}
}
