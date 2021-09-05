using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableToLookup {
		private enum MonsterCategory {
			None = 0,
			Slime,
			Animal,
			Fly,
		}

		private record Monster(string Name, MonsterCategory Category);

		[Fact]
		public void ToLookup_使ってみる() {
			// Arrange
			var monsters = new[] {
				new Monster("スライム", MonsterCategory.Slime),
				new Monster("ドラキー", MonsterCategory.Fly),
				new Monster("ホイミスライム", MonsterCategory.Slime),
				new Monster("ももんじゃ", MonsterCategory.Animal),
			};

			// Act
			var lookup = monsters.ToLookup(monster => monster.Category);

			// Assert
			// Countプロパティ：グルーピングされたグループの個数を取得できる
			Assert.Equal(3, lookup.Count);

			// Containsメソッド：キーを含んでいる判断できる
			Assert.False(lookup.Contains(MonsterCategory.None));
			Assert.True(lookup.Contains(MonsterCategory.Slime));
			Assert.True(lookup.Contains(MonsterCategory.Animal));
			Assert.True(lookup.Contains(MonsterCategory.Fly));
		}
	}
}
