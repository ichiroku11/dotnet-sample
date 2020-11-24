using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace SampleTest {
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
	}
}
