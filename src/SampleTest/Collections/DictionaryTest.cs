using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest.Collections {
	public class DictionaryTest {
		[Fact]
		public void Add_同じキーを追加すると例外がスローされる() {
			// Arrange
			var source = new Dictionary<string, string>() {
				{ "a", "A" },
				{ "b", "B" },
			};

			// Act
			// Assert
			Assert.Throws<ArgumentException>(() => {
				new Dictionary<string, string>(source).Add("a", "X");
			});
		}

		[Fact]
		public void CollectionInitializer_コレクション初期化子で同じキーを追加しても例外がスローされる() {
			// Arrange
			var source = new Dictionary<string, string>() {
				{ "a", "A" },
				{ "b", "B" },
			};

			// Act
			// Assert
			Assert.Throws<ArgumentException>(() => {
				new Dictionary<string, string>(source) {
					{ "a", "X" }
				};
			});
		}
	}
}
