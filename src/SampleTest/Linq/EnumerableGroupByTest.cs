using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.Linq {
	public class EnumerableGroupByTest {
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

		// IGroupingはKeyプリパティがあるコレクション
		/*
		public interface IGrouping<out TKey, out TElement> : IEnumerable<TElement>, IEnumerable {
			TKey Key { get; }
		}
		*/

		[Fact]
		public void GroupBy_Groupingを列挙する() {
			// Arrange

			// Act
			// keySelectorによるキーと該当する要素のコレクションに変換する
			// groupsはIGroupingのコレクション（IEnumerable<IGrouping<MonsterCategory, Monster>>）
			var groups = _monsters.GroupBy(monster => monster.Category);

			// Assert
			Assert.Collection(
				groups.OrderBy(entry => entry.Key),
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
	}
}
