using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq;

public class EnumerableSingleOrDefaultTest {
	[Fact]
	public void Single_シーケンスが空だとInvalidOperationExceptionがスロー() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<InvalidOperationException>(() => {
			Enumerable.Empty<int>().Single();
		});
	}

	[Fact]
	public void Single_シーケンスの要素が2つ以上だとInvalidOperationExceptionがスロー() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<InvalidOperationException>(() => {
			Enumerable.Repeat(0, 2).Single();
		});
	}

	[Fact]
	public void SingleOrDefault_シーケンスが空の場合は例外がスローされずデフォルト値を取得できる() {
		// Arrange
		// Act
		var value = Enumerable.Empty<int>().SingleOrDefault();
		// Assert
		Assert.Equal(default, value);
	}

	[Fact]
	public void SingleOrDefault_シーケンスの要素が2つ以上だとInvalidOperationExceptionがスロー() {
		// Arrange
		// Act
		// Assert
		Assert.Throws<InvalidOperationException>(() => {
			Enumerable.Repeat(0, 2).SingleOrDefault();
		});
	}
}
