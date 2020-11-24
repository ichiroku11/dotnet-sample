using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.Reflection {
	public class TypeInfoTest {
		public class Sample {
			public int Value { get; set; }
		}

		private readonly ITestOutputHelper _output;

		public TypeInfoTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void GetMethods_親クラスのメソッドを取得できる() {
			// Arrange
			// Act
			var methods = typeof(Sample).GetTypeInfo()
				.GetMethods()
				.Where(method => !method.GetCustomAttributes<CompilerGeneratedAttribute>().Any());

			// Assert
			Assert.Equal(4, methods.Count());
			Assert.All(methods, method => {
				_output.WriteLine(method.Name);
			});
			var sample = new Sample();
			Assert.Contains(methods, method => string.Equals(method.Name, nameof(sample.Equals)));
			Assert.Contains(methods, method => string.Equals(method.Name, nameof(sample.GetHashCode)));
			Assert.Contains(methods, method => string.Equals(method.Name, nameof(sample.GetType)));
			Assert.Contains(methods, method => string.Equals(method.Name, nameof(sample.ToString)));
		}

		[Fact]
		public void GetMethods_GetterSetterプロパティは対応するメソッドが自動生成される() {
			// Arrange
			// Act
			var methods = typeof(Sample).GetTypeInfo()
				.GetMethods()
				.Where(method => method.GetCustomAttributes<CompilerGeneratedAttribute>().Any());

			// Assert
			Assert.Equal(2, methods.Count());
			Assert.All(methods, method => {
				_output.WriteLine(method.Name);
				Assert.True(method.IsSpecialName);
			});
			Assert.Contains(methods, method => string.Equals(method.Name, $"get_{nameof(Sample.Value)}"));
			Assert.Contains(methods, method => string.Equals(method.Name, $"set_{nameof(Sample.Value)}"));
		}
	}
}
