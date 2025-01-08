namespace SampleTest.Linq;

public class EnumerableCastTest {
	private interface ISample {
	}

	private class Sample : ISample {
	}

	[Fact]
	public void Cast_castする() {
		// Arrange
		var values = new object[] {
			new Sample(),
			new { },
		};

		// Act
		var actual = values.Cast<ISample>();

		// Assert
		// 1つ目はcastできるので取得できる
		Assert.IsType<Sample>(actual.First());

		// 2つ目を取得しようとするとcastできないので例外が発生する
		Assert.Throws<InvalidCastException>(() => {
			actual.Skip(1).First();
		});
	}
}
