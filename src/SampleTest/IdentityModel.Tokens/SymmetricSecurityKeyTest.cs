using Microsoft.IdentityModel.Tokens;

namespace SampleTest.IdentityModel.Tokens;

public class SymmetricSecurityKeyTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	[Fact]
	public void Constructor_キーの長さが0だと例外が発生する() {
		var exception = Assert.Throws<ArgumentException>(() => {
			new SymmetricSecurityKey(Array.Empty<byte>());
		});
		_output.WriteLine(exception.Message);
	}
}
