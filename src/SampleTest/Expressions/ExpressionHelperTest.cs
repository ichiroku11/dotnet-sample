using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace SampleTest.Expressions {
	public class ExpressionHelperTest {
		// テスト用
		public class Item {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		[Fact]
		public void GetMemberName_参照型のプロパティにアクセスする式からプロパティ名を取得() {
			var member = ExpressionHelper.GetMemberName<Item>(item => item.Name);

			Assert.Equal("Name", member);
		}

		[Fact]
		public void GetMemberName_値型のプロパティにアクセスする式からプロパティ名を取得() {
			var member = ExpressionHelper.GetMemberName<Item>(item => item.Id);

			Assert.Equal("Id", member);
		}

		[Fact]
		public void GetMemberNames_匿名オブジェクトを生成する式からメンバー名一覧を取得() {
			var members = ExpressionHelper.GetMemberNames<Item>(item => new { item.Id, item.Name });

			Assert.Collection(members,
				member => Assert.Equal("Id", member),
				member => Assert.Equal("Name", member));
		}
	}
}
