using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq;

public class EnumerableDefaultIfEmptyTest {
	[Fact]
	public void DefaultIfEmpty_シーケンスが空の場合はデフォルト値1つのコレクションを返す() {
		// Arrange
		var values = new List<int>();

		// Act
		var actual = values.DefaultIfEmpty();

		// Assert
		Assert.Equal(new[] { 0 }, actual);
	}

	[Fact]
	public void DefaultIfEmpty_シーケンスが空の場合は指定した値1つのコレクションを返す() {
		// Arrange
		var values = new List<int>();

		// Act
		var actual = values.DefaultIfEmpty(-1);

		// Assert
		Assert.Equal(new[] { -1 }, actual);
	}

	[Fact]
	public void DefaultIfEmpty_シーケンスに要素が含まれている場合はそのシーケンスを返す() {
		// Arrange
		var values = new List<int> { 1 };

		// Act
		var actual = values.DefaultIfEmpty();

		// Assert
		Assert.Equal(new[] { 1 }, actual);
	}
}
