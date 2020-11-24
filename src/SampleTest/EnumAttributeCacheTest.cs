using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace SampleTest {
	public class EnumAttributeCacheTest {
		private enum Fruit {
			None = 0,

			[Display(Name = "りんご")]
			Apple,

			[Display(Name = "バナナ")]
			Banana,
		}

		private enum Vegetable {
			None = 0,

			[Display(Name = "たまねぎ")]
			Onion,

			[Display(Name = "にんじん")]
			Carrot
		}

		[Fact]
		public void Get_取得できるその1() {
			// Arrange
			// Act
			var attribute = EnumAttributeCache<Fruit, DisplayAttribute>.Get(Fruit.None);

			// Assert
			Assert.Null(attribute);
		}

		[Fact]
		public void Get_取得できるその2() {
			// Arrange
			// Act
			// Fruitの属性が遅延実行でキャッシュされる
			var attribute1 = EnumAttributeCache<Fruit, DisplayAttribute>.Get(Fruit.Apple);
			var attribute2 = EnumAttributeCache<Fruit, DisplayAttribute>.Get(Fruit.Banana);
			// Vegetableの属性が遅延実行でキャッシュされる
			var attribute3 = EnumAttributeCache<Vegetable, DisplayAttribute>.Get(Vegetable.Onion);
			var attribute4 = EnumAttributeCache<Vegetable, DisplayAttribute>.Get(Vegetable.Carrot);

			// Assert
			Assert.Equal("りんご", attribute1.Name);
			Assert.Equal("バナナ", attribute2.Name);
			Assert.Equal("たまねぎ", attribute3.Name);
			Assert.Equal("にんじん", attribute4.Name);
		}
	}
}
