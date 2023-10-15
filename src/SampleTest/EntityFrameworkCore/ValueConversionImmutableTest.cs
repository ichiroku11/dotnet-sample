using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 値オブジェクトにマッピングするサンプル
// 参考
// https://docs.microsoft.com/ja-jp/ef/core/modeling/value-conversions
public class ValueConversionImmutableTest {
	// 値オブジェクト
	private class UserName : IEquatable<UserName> {
		public UserName(string value) {
			Value = value;
		}

		public string Value { get; }

		public override bool Equals(object? obj) => Equals(obj as UserName);

		public bool Equals(UserName? other) => other != null && Value == other.Value;

		public override int GetHashCode() => HashCode.Combine(Value);
	}

	private class User {
		public int Id { get; init; }
		public UserName Name { get; init; } = new UserName("");
	}

	private class UserDbContext : SqlServerDbContext {
		public DbSet<User> Users => Set<User>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<User>()
				.ToTable(nameof(User))
				.Property(user => user.Name)
				.HasConversion(
					userName => userName.Value,
					value => new UserName(value));
		}
	}

	// 1カラム1プロパティ
	[Fact]
	public async Task シンプルな値オブジェクトにマッピングする() {
		using var context = new UserDbContext();
		var user = await context.Users
			.FromSqlInterpolated($"select {1} as Id, {"a"} as Name")
			.FirstOrDefaultAsync();

		Assert.Equal(1, user?.Id);
		Assert.Equal("a", user?.Name?.Value);
	}

	// 本当は追加や更新も試したいけど
}
