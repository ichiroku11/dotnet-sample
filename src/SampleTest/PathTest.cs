namespace SampleTest;

public class PathTest {
	[Fact]
	public void GetTempFileName_メソッドを呼び出すと空のファイルが作られる() {
		var path = "";
		try {
			path = Path.GetTempFileName();

			// 空のファイルができる
			Assert.True(File.Exists(path));
			Assert.Empty(File.ReadAllBytes(path));
		} finally {
			// 後始末
			File.Delete(path);
		}
	}

	[Fact]
	public void GetFullPath_ファイル名を指定するとカレントディレクトリベースのフルパスを取得する() {
		// Arrange
		var expected = Path.Combine(Directory.GetCurrentDirectory(), "readme.md");

		// Act
		var actual = Path.GetFullPath("readme.md");

		// Assert
		Assert.Equal(expected, actual);
	}
}
