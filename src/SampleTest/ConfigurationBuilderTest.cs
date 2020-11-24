using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;

namespace SampleTest {
	public class ConfigurationBuilderTest {
		// オプション
		private class Options {
			public string Sample { get; set; }
		}

		// オプションのjsonストリームを取得
		public static Stream GetConfigJsonStream(string value) {
			// Dispooseは呼び出し側で
			var stream = new MemoryStream();

			// jsonのバイト配列にシリアライズして書き込む
			var json = JsonSerializer.SerializeToUtf8Bytes(
				new Options {
					Sample = value
				});
			stream.Write(json, 0, json.Length);

			stream.Position = 0;

			return stream;
		}

		[Fact]
		public void AddJsonStream_構成を2つ読み込むと後勝ち() {
			// Arrange
			using var stream1 = GetConfigJsonStream("abc");
			using var stream2 = GetConfigJsonStream("efg");

			// Action
			var builder = new ConfigurationBuilder()
				.AddJsonStream(stream1)
				// 上書きする
				.AddJsonStream(stream2);
			var options = builder.Build().Get<Options>();

			// Assert
			Assert.Equal("efg", options.Sample);
		}

		[Fact]
		public void AddConfiguration_構成を2つ読み込むと後勝ち() {
			// Arrange
			using var stream1 = GetConfigJsonStream("abc");
			using var stream2 = GetConfigJsonStream("efg");

			// Action
			var config1 = new ConfigurationBuilder()
				.AddJsonStream(stream1)
				.Build();

			// Assert
			Assert.Equal("abc", config1.Get<Options>().Sample);

			// Action
			var config2 = new ConfigurationBuilder()
				// config1を使う
				.AddConfiguration(config1)
				// 上書きする
				.AddJsonStream(stream2)
				.Build();

			// Assert
			Assert.Equal("abc", config1.Get<Options>().Sample);
			Assert.Equal("efg", config2.Get<Options>().Sample);
		}
	}
}
