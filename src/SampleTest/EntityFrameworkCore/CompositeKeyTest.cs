using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SampleTest.EntityFrameworkCore {
	// 複合主キー、複合外部キーを試す
	// 参考
	// https://docs.microsoft.com/ja-jp/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-composite-key%2Csimple-key#foreign-key
	[Collection(CollectionNames.EfCoreSample)]
	public class CompositeKeyTest : IDisposable {
		private class Sample {
			public int Id { get; init; }

			public string Value { get; init; }

			public List<SampleDetail> Details { get; init; }
		}

		private class SampleDetail {
			public int SampleId { get; init; }

			public int DetailNo { get; init; }

			public string Value { get; init; }
		}

		public void Dispose() {
			// todo:
		}
	}
}
