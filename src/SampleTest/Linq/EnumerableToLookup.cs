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
			// keySelectorによるキーと該当する要素のコレクションに変換する
			var lookup = monsters.ToLookup(monster => monster.Category);

			// Assert
			// Countプロパティ：グルーピングされたグループの個数を取得できる
			Assert.Equal(3, lookup.Count);

			// Containsメソッド：キーを含んでいる判断できる
			Assert.False(lookup.Contains(MonsterCategory.None));
			Assert.True(lookup.Contains(MonsterCategory.Slime));
			Assert.True(lookup.Contains(MonsterCategory.Animal));
			Assert.True(lookup.Contains(MonsterCategory.Fly));

			// インデクサ：含まれないキーでアクセスした場合、nullではなく空のコレクションを取得できる
			Assert.NotNull(lookup[MonsterCategory.None]);
			Assert.Empty(lookup[MonsterCategory.None]);

			// インデクサ：含まれるキーでアクセスした場合、要素のコレクションを取得できる
			var slimes = lookup[MonsterCategory.Slime];
			Assert.Equal(2, slimes.Count());
			Assert.Contains(slimes, slime => slime.Name == "スライム");
			Assert.Contains(slimes, slime => slime.Name == "ホイミスライム");

			Assert.Single(lookup[MonsterCategory.Animal]);
			Assert.Contains(lookup[MonsterCategory.Animal], animal => animal.Name == "ももんじゃ");

			Assert.Single(lookup[MonsterCategory.Fly]);
			Assert.Contains(lookup[MonsterCategory.Fly], animal => animal.Name == "ドラキー");
		}
	}
}
