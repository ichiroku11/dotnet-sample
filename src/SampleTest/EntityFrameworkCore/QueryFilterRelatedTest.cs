using Microsoft.EntityFrameworkCore;
using Xunit;

namespace SampleTest.EntityFrameworkCore;

// https://docs.microsoft.com/ja-jp/ef/core/querying/related-data/eager#filtered-include

// グローバルフィルターがIncludeにも適用されるか確認する
// todo:
//[Collection(CollectionNames.EfCoreSample)]
public class QueryFilterRelatedTest : IDisposable {
	// todo:

	private class SampleDbContext : SqlServerDbContext {
		protected override void OnModelCreating(ModelBuilder modelBuilder) {
		}
	}

	private readonly SampleDbContext _context = new();

	public void Dispose() {
		_context.Dispose();
	}
}
