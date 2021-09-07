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

		private static readonly IEnumerable<Monster> _monsters = new[] {
			new Monster("スライム", MonsterCategory.Slime),
			new Monster("ドラキー", MonsterCategory.Fly),
			new Monster("ホイミスライム", MonsterCategory.Slime),
			new Monster("ももんじゃ", MonsterCategory.Animal),
		};

		// ToLookupメソッドで取得できるILookupは、
		// IGroupingのコレクションにインデクサやContainsメソッドなどを追加したインターフェイス
		/*
		public interface ILookup<TKey, TElement> : IEnumerable<IGrouping<TKey, TElement>>, IEnumerable {
			IEnumerable<TElement> this[TKey key] { get; }
			int Count { get; }
			bool Contains(TKey key);
		}
		*/

		[Fact]
		public void ToLookup_Lookupの要素を列挙する() {
			// Arrange

			// Act
			// keySelectorによるキーと該当する要素のコレクションに変換する
			var lookup = _monsters.ToLookup(monster => monster.Category);

			// Assert
			Assert.Collection(
				lookup.OrderBy(entry => entry.Key),
				entry => {
					Assert.Equal(MonsterCategory.Slime, entry.Key);
					Assert.Equal(2, entry.Count());
					Assert.Contains(entry, item => item.Name.Equals("スライム", StringComparison.OrdinalIgnoreCase));
					Assert.Contains(entry, item => item.Name.Equals("ホイミスライム", StringComparison.OrdinalIgnoreCase));
				},
				entry => {
					Assert.Equal(MonsterCategory.Animal, entry.Key);
					Assert.Single(entry);
					Assert.Contains(entry, item => item.Name.Equals("ももんじゃ", StringComparison.OrdinalIgnoreCase));
				},
				entry => {
					Assert.Equal(MonsterCategory.Fly, entry.Key);
					Assert.Single(entry);
					Assert.Contains(entry, item => item.Name.Equals("ドラキー", StringComparison.OrdinalIgnoreCase));
				});
		}

		[Fact]
		public void ToLookup_LookupのCountプロパティを試す() {
			// Arrange

			// Act
			var lookup = _monsters.ToLookup(monster => monster.Category);

			// Assert

			// Countプロパティ：グルーピングされたグループの個数を取得できる
			Assert.Equal(3, lookup.Count);
		}

		[Fact]
		public void ToLookup_LookupのContainsメソッドを試す() {
			// Arrange
			// Act
			var lookup = _monsters.ToLookup(monster => monster.Category);

			// Assert
			// Containsメソッド：キーを含んでいる判断できる
			Assert.False(lookup.Contains(MonsterCategory.None));
			Assert.True(lookup.Contains(MonsterCategory.Slime));
			Assert.True(lookup.Contains(MonsterCategory.Animal));
			Assert.True(lookup.Contains(MonsterCategory.Fly));
		}

		[Fact]
		public void ToLookup_Lookupのインデクサを試す() {
			// Arrange
			// Act
			var lookup = _monsters.ToLookup(monster => monster.Category);

			// Assert
			// インデクサ：含まれないキーでアクセスした場合、nullではなく空のコレクションを取得できる
			Assert.NotNull(lookup[MonsterCategory.None]);
			Assert.Empty(lookup[MonsterCategory.None]);

			// インデクサ：含まれるキーでアクセスした場合、要素のコレクションを取得できる
			var slimes = lookup[MonsterCategory.Slime];
			Assert.Equal(2, slimes.Count());
			Assert.Contains(slimes, slime => slime.Name.Equals("スライム", StringComparison.OrdinalIgnoreCase));
			Assert.Contains(slimes, slime => slime.Name.Equals("ホイミスライム", StringComparison.OrdinalIgnoreCase));

			Assert.Single(lookup[MonsterCategory.Animal]);
			Assert.Contains(lookup[MonsterCategory.Animal], animal => animal.Name.Equals("ももんじゃ", StringComparison.OrdinalIgnoreCase));

			Assert.Single(lookup[MonsterCategory.Fly]);
			Assert.Contains(lookup[MonsterCategory.Fly], animal => animal.Name.Equals("ドラキー", StringComparison.OrdinalIgnoreCase));
		}
	}
}
