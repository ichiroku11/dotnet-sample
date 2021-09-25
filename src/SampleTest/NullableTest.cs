using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest {
	public class NullableTest {
		// https://docs.microsoft.com/ja-jp/dotnet/csharp/language-reference/builtin-types/nullable-value-types#how-to-identify-a-nullable-value-type

		private readonly ITestOutputHelper _output;

		public NullableTest(ITestOutputHelper output) {
			_output = output;
		}

		[Theory]
		[InlineData(typeof(int?), "Int32")]
		[InlineData(typeof(long?), "Int64")]
		public void GetUnderlyingType_引数にnull許容型の型を指定すると型引数の型を取得できる(Type type, string expectedName) {
			// Arrange
			// Act
			// Returns the underlying type argument of the specified nullable type.
			var actual = Nullable.GetUnderlyingType(type);
			_output.WriteLine(actual.Name);

			// Assert
			Assert.Equal(expectedName, actual.Name);
		}

		[Theory]
		[InlineData(typeof(int))]
		[InlineData(typeof(long))]
		public void GetUnderlyingType_引数にnull許容型ではない型を指定するとnullを返す(Type type) {
			// Arrange
			// Act
			var actual = Nullable.GetUnderlyingType(type);

			// Assert
			Assert.Null(actual);
		}
	}
}
