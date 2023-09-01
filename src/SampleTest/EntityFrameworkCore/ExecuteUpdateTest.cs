namespace SampleTest.EntityFrameworkCore;

// https://learn.microsoft.com/ja-jp/ef/core/saving/execute-insert-update-delete
[Collection(CollectionNames.EfCoreSample)]
public class ExecuteUpdateTest : IDisposable {

	private class Sample {
		public int Id { get; set; }
		public string Name { get; set; } = "";
	}


	public void Dispose() {
		GC.SuppressFinalize(this);
	}
}
