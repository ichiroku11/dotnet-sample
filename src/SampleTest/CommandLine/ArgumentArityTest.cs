using System.CommandLine;

namespace SampleTest.CommandLine;

public class ArgumentArityTest {
	[Fact]
	public void MinimumNumberOfValues_MaximumNumberOfValues_各スタティックインスタンスの最大値と最小値を確認する() {
		// Arrange
		// Act
		// Assert
		Assert.Equal(0, ArgumentArity.Zero.MinimumNumberOfValues);
		Assert.Equal(0, ArgumentArity.Zero.MaximumNumberOfValues);

		Assert.Equal(1, ArgumentArity.ExactlyOne.MinimumNumberOfValues);
		Assert.Equal(1, ArgumentArity.ExactlyOne.MaximumNumberOfValues);

		Assert.Equal(0, ArgumentArity.ZeroOrOne.MinimumNumberOfValues);
		Assert.Equal(1, ArgumentArity.ZeroOrOne.MaximumNumberOfValues);

		Assert.Equal(0, ArgumentArity.ZeroOrMore.MinimumNumberOfValues);
		Assert.Equal(100_000, ArgumentArity.ZeroOrMore.MaximumNumberOfValues);

		Assert.Equal(1, ArgumentArity.OneOrMore.MinimumNumberOfValues);
		Assert.Equal(100_000, ArgumentArity.OneOrMore.MaximumNumberOfValues);
	}
}
