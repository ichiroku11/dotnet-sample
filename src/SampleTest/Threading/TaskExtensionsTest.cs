using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Threading {
	public class TaskExtensionsTest {
		[Fact]
		public async void FireAndForget_試してみる() {
			// Arrange
			// Act
			// Assert
			var called = false;
			await Task.FromException(new ArgumentOutOfRangeException())
				.FireAndForget(exception => {
					Assert.IsType<ArgumentOutOfRangeException>(exception);
					called = true;
				});
			Assert.True(called);
		}
	}
}
