using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace SampleTest.Expressions {
	public class MapHelperTest {
		// テスト用：コピー元
		private class From {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		// テスト用：コピー先
		private class To {
			public int Id { get; set; }
			public string Name { get; set; }
		}

		[Fact]
		public void CreateMapper_AutoMapperみたいな機能を構築する() {
			var mapper = MapHelper.CreateMapper<From, To>();

			var from = new From { Id = 1, Name = "x" };
			var to = mapper(from);

			Assert.Equal(from.Id, to.Id);
			Assert.Equal(from.Name, to.Name);
		}
	}
}
