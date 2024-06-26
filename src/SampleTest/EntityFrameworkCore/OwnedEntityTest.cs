using Microsoft.EntityFrameworkCore;

namespace SampleTest.EntityFrameworkCore;

// 所有型を試してみる
// 参考
// https://docs.microsoft.com/ja-jp/ef/core/modeling/owned-entities
public class OwnedEntityTest : IDisposable {
	// メールアドレス
	private record MailAddress(string Address, string Name) {
		public static readonly MailAddress Empty = new("", "");
	}

	// メール
	private record Mail {
		public int Id { get; init; }
		public MailAddress From { get; init; } = MailAddress.Empty;
		public MailAddress To { get; init; } = MailAddress.Empty;
	}
	// このrecord型は無理っぽい（なぜ）
	// private record Mail(int Id, MailAddress From, MailAddress To);

	private class MailDbContext : SqlServerDbContext {
		public DbSet<Mail> Mails => Set<Mail>();

		protected override void OnModelCreating(ModelBuilder modelBuilder) {
			modelBuilder.Entity<Mail>().ToTable(nameof(Mail))
				.OwnsOne(mail => mail.From, ownedBuilder => {
						// カラム名を指定
						ownedBuilder.Property(address => address.Address)
						.HasColumnName($"{nameof(Mail.From)}{nameof(MailAddress.Address)}");
					ownedBuilder.Property(address => address.Name)
						.HasColumnName($"{nameof(Mail.From)}{nameof(MailAddress.Name)}");
				})
				.OwnsOne(mail => mail.To, ownedBuilder => {
						// カラム名を指定
						ownedBuilder.Property(address => address.Address)
						.HasColumnName($"{nameof(Mail.To)}{nameof(MailAddress.Address)}");
					ownedBuilder.Property(address => address.Name)
						.HasColumnName($"{nameof(Mail.To)}{nameof(MailAddress.Name)}");
				});
		}
	}

	private readonly MailDbContext _context = new();

	public OwnedEntityTest() {
		DropTable();
		InitTable();
	}

	public void Dispose() {
		DropTable();

		_context.Dispose();

		GC.SuppressFinalize(this);
	}

	private void InitTable() {
		FormattableString sql = $@"
create table dbo.Mail(
	Id int not null,
	FromAddress nvarchar(20) not null,
	FromName nvarchar(10) not null,
	ToAddress nvarchar(20) not null,
	ToName nvarchar(10) not null,
	constraint PK_Mail primary key(Id)
);

insert into dbo.Mail(Id, FromAddress, FromName, ToAddress, ToName)
output inserted.*
values
	(1, N'from@example.jp', N'送信元', N'to@example.jp', N'宛先');";

		_context.Database.ExecuteSql(sql);
	}

	private void DropTable() {
		FormattableString sql = $@"
drop table if exists dbo.Mail;";

		_context.Database.ExecuteSql(sql);
	}

	[Fact]
	public async Task FirstOrDefault_所有型を取得できる() {
		// Arrange
		var expected = new Mail {
			Id = 1,
			From = new MailAddress("from@example.jp", "送信元"),
			To = new MailAddress("to@example.jp", "宛先"),
		};

		// Act
		var actual = await _context.Mails.FirstOrDefaultAsync(mail => mail.Id == 1);

		// Assert
		Assert.Equal(expected, actual);
	}
}
