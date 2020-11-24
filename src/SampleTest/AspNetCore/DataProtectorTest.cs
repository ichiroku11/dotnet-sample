using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace SampleTest.AspNetCore {
	public class DataProtectorTest {
		private static IServiceProvider GetServiceProvider() {
			var services = new ServiceCollection();
			services.AddDataProtection();

			return services.BuildServiceProvider();
		}

		private readonly ITestOutputHelper _output;

		public DataProtectorTest(ITestOutputHelper output) {
			_output = output;
		}

		[Fact]
		public void ProtectUnprotect_暗号化で保護データを復号できる() {
			// Arrange
			var protector = GetServiceProvider().GetDataProtectionProvider().CreateProtector(nameof(DataProtectorTest));
			var expected = "Hello world!";

			// Act
			// 平文を保護する
			var protectedData = protector.Protect(expected);

			_output.WriteLine(protectedData);

			// 保護を解除して平文を取り出す
			var actual = protector.Unprotect(protectedData);

			// Assert
			Assert.Equal(expected, actual);
		}
	}
}
