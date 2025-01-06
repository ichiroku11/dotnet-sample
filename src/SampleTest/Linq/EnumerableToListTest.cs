using System.Linq;

namespace SampleTest.Linq;

public class EnumerableToListTest {
	private interface ISample {
	}

	private class Sample : ISample {
	}

	[Fact]
	public void ToList_クラスのListをインターフェイスのIListに変換する() {
		// Arrange
		var items = new List<Sample> { new() };

		// Act
		// Assert
		Assert.True(items.ToList<ISample>() is List<ISample>);
	}
}
