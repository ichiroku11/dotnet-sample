using Microsoft.Extensions.FileProviders;

namespace SampleTest.Extensions.FileProviders;

public class PhysicalFileProviderTest : IDisposable {
	private static readonly string _tempFolder = Path.Combine(Directory.GetCurrentDirectory(), $"${nameof(PhysicalFileProviderTest)}");

	private readonly ITestOutputHelper _output;

	public PhysicalFileProviderTest(ITestOutputHelper output) {
		_output = output;

		// 作業フォルダ
		Directory.CreateDirectory(_tempFolder);
		_output.WriteLine(_tempFolder);

		// テスト用ファイル
		File.WriteAllText(Path.Combine(_tempFolder, "test.txt"), "test");
	}

	public void Dispose() {
		Directory.Delete(_tempFolder, true);
	}

	[Fact]
	public void Constructor_存在しないフォルダパスを引数に指定すると例外がスローされる() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<DirectoryNotFoundException>(() => {
			new PhysicalFileProvider(Path.Combine(_tempFolder, "notfound"));
		});
	}

	[Fact]
	public async Task GetFileInfo_ファイルを取得できる() {
		// Arrange
		var fileProvider = new PhysicalFileProvider(_tempFolder);

		// Act
		var fileInfo = fileProvider.GetFileInfo("test.txt");

		using var stream = fileInfo.CreateReadStream();
		using var reader = new StreamReader(stream);
		var content = await reader.ReadToEndAsync();

		// Assert
		Assert.True(fileInfo.Exists);
		Assert.Equal("test", content);
	}

	[Fact]
	public void GetFileInfo_スコープ外のファイルは存在していてもアクセスできない() {
		// Arrange
		var subFolder = Path.Combine(_tempFolder, "sub");
		Directory.CreateDirectory(subFolder);

		var fileProvider = new PhysicalFileProvider(subFolder);

		// Act
		var fileInfo = fileProvider.GetFileInfo("test.txt");

		// Assert
		// 実際にはファイルは存在するがIFileInfoから見つけられない
		Assert.True(File.Exists(Path.Combine(_tempFolder, "test.txt")));
		Assert.False(fileInfo.Exists);
	}
}
