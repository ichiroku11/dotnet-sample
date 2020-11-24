using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Xunit;

namespace SampleTest {
	public class EnumHelperTest {
		private enum Fruit {
			None = 0,

			[Display(Name = "りんご")]
			Apple,

			[Display(Name = "バナナ")]
			Banana,
		}

		[Fact]
		public void GetAttributes_属性を取得できる() {
			// Arrange
			// Act
			var attributes = EnumHelper.GetAttributes<Fruit, DisplayAttribute>();

			// Assert
			Assert.Equal(3, attributes.Count);
			Assert.Null(attributes[Fruit.None]);
			Assert.Equal("りんご", attributes[Fruit.Apple].Name);
			Assert.Equal("バナナ", attributes[Fruit.Banana].Name);
		}

		[Fact]
		public void GetValues_値を取得できる() {
			// Arrange
			// Act
			var fruits = EnumHelper.GetValues<Fruit>();

			// Assert
			Assert.Equal(3, fruits.Count());
			Assert.Contains(Fruit.None,fruits);
			Assert.Contains(Fruit.Apple, fruits);
			Assert.Contains(Fruit.Banana, fruits);
		}
	}
}
