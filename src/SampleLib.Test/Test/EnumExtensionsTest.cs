using System.ComponentModel.DataAnnotations;
using Xunit;

namespace SampleLib.Test;

public class EnumExtensionsTest {
	private enum Fruit {
		None = 0,

		[Display(Name = "りんご")]
		Apple,
	}

	[Fact]
	public void DisplayName_取得できる() {
		// Arrange
		// Act
		// Assert
		Assert.Null(Fruit.None.DisplayName());
		Assert.Equal("りんご", Fruit.Apple.DisplayName());
	}
}
