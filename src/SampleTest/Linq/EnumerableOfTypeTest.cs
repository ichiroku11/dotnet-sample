namespace SampleTest.Linq;

public class EnumerableOfTypeTest {
	private interface ISample {
	}

	private class Sample : ISample {
	}

	[Fact]
	public void OfType_castできる要素にフィルターする() {
		// Arrange
		var values = new object[] {
			new Sample(),
			new { },
		};

		// Act
		var actual = values.OfType<ISample>();

		// Assert
		// 変換できる（castできる）要素のみにフィルターされる
		var sample = Assert.Single(actual);
		Assert.IsType<Sample>(sample);
	}
}
