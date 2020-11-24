using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace SampleTest {
	public class EnumTest {
		private enum Fruit {
			[Display(Name = "りんご")]
			Apple = 1,

			[Display(Name = "バナナ")]
			Banana,

			[Display(Name = "オレンジ")]
			Orange,
		}

		private static readonly IEnumerable<Fruit> _fruits
			= new[] {
				Fruit.Apple,
				Fruit.Banana,
				Fruit.Orange
			};

		[Flags]
		private enum Attributes : byte {
			None = 0,
			Read = 0b0001,
			Write = 0b0010,
			Execute = 0b0100,
			ReadWrite = Read | Write,
		}

		[Fact]
		public void IsDefined_数値を識別できる() {
			// Arrange
			// Act
			// Assert

			// 定義されている
			Assert.True(Enum.IsDefined(typeof(Fruit), (int)Fruit.Apple));

			// 定義されていない
			Assert.False(Enum.IsDefined(typeof(Fruit), (int)Fruit.Orange + 1));
		}

		[Fact]
		public void IsDefined_文字列を識別できる() {
			// Arrange
			// Act
			// Assert
			Assert.True(Enum.IsDefined(typeof(Fruit), "Apple"));
		}

		[Fact]
		public void GetValues_enum値を列挙できる() {
			// Arrange
			// Act
			var fruits = Enum.GetValues(typeof(Fruit)).OfType<Fruit>();

			// Assert
			Assert.Equal(_fruits, fruits);
		}

		[Fact]
		public void Parse_文字列をEnumに変換できる() {
			// Arrange
			// Act
			var apple = Enum.Parse<Fruit>("apple", ignoreCase: true);

			// Assert
			Assert.Equal(Fruit.Apple, apple);
		}

		[Fact]
		public void HasFlag_フラグが設定されているか確認する() {
			// Arrange
			var attribute = Attributes.ReadWrite;

			// Act
			// Assert
			Assert.True(attribute.HasFlag(Attributes.Read));
			Assert.True(attribute.HasFlag(Attributes.Write));
			Assert.True(attribute.HasFlag(Attributes.ReadWrite));
			Assert.False(attribute.HasFlag(Attributes.Execute));
		}

		[Fact]
		public void BitwiseOperator_フラグを設定する() {
			// Arrange
			var attribute = Attributes.Read;
			attribute |= Attributes.Write;

			// Act
			// Assert
			Assert.True(attribute.HasFlag(Attributes.Read));
			Assert.True(attribute.HasFlag(Attributes.Write));
			Assert.True(attribute.HasFlag(Attributes.ReadWrite));
		}

		[Fact]
		public void BitwiseOperator_フラグをクリアする() {
			// Arrange
			var attribute = Attributes.Read | Attributes.Execute;
			attribute &= ~Attributes.Execute;

			// Act
			// Assert
			Assert.True(attribute.HasFlag(Attributes.Read));
			// 元から設定されていない
			Assert.False(attribute.HasFlag(Attributes.Write));
			Assert.False(attribute.HasFlag(Attributes.Execute));
		}

		[Fact]
		public void BitwiseOperator_フラグをクリアする_設定されていない値をクリアしても問題ない() {
			// Arrange
			var attribute = Attributes.Read;
			attribute &= ~(Attributes.Write | Attributes.Execute);

			// Act
			// Assert
			Assert.True(attribute.HasFlag(Attributes.Read));
			Assert.False(attribute.HasFlag(Attributes.Write));
			Assert.False(attribute.HasFlag(Attributes.Execute));
		}

		[Fact]
		public void リフレクションでenum値を列挙する() {
			// Arrange
			// Act
			var fruits = typeof(Fruit)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.Select(field => (Fruit)field.GetValue(null));

			// Assert
			Assert.Equal(_fruits, fruits);
		}

		[Fact]
		public void リフレクションでDisplayAttributeを取得する() {
			// Arrange
			// Act
			var displayAttribute = typeof(Fruit)
				.GetField(nameof(Fruit.Apple))
				.GetCustomAttributes<DisplayAttribute>()
				.First();

			// Assert
			Assert.Equal("りんご", displayAttribute.Name);
		}

		[Fact]
		public void リフレクションでenumとDisplayAttributeのDictionaryを作る() {
			// Arrange
			// Act
			var displayAttributes = typeof(Fruit)
				.GetFields(BindingFlags.Public | BindingFlags.Static)
				.ToDictionary(
					field => (Fruit)field.GetValue(null),
					field => field.GetCustomAttributes<DisplayAttribute>().First());

			// Assert
			Assert.Equal(3, displayAttributes.Count);
			Assert.Equal("りんご", displayAttributes[Fruit.Apple].Name);
			Assert.Equal("バナナ", displayAttributes[Fruit.Banana].Name);
			Assert.Equal("オレンジ", displayAttributes[Fruit.Orange].Name);
		}
	}
}
