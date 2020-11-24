using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest {
	public class AsOperatorTest {
		[Fact]
		public void AsOperator_Int32のListはInt32のIListに変換できる() {
			// Arrange
			var items = new List<int> { 1 };

			// Act
			// Assert
			Assert.NotNull(items as IList<int>);
		}

		private interface ISample {
		}

		private class Sample : ISample {
		}

		[Fact]
		public void AsOperator_クラスのListはクラスのIListに変換できる() {
			// Arrange
			var items = new List<Sample> { new Sample() };

			// Act
			// Assert
			Assert.NotNull(items as IList<Sample>);
		}

		[Fact]
		public void AsOperator_クラスのListはインターフェイスのIListに変換できない() {
			// Arrange
			var items = new List<Sample> { new Sample() };

			// Act
			// Assert
			Assert.Null(items as IList<ISample>);
		}
	}
}
