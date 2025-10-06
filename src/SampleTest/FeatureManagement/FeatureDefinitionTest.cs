using Microsoft.FeatureManagement;

namespace SampleTest.FeatureManagement;

public class FeatureDefinitionTest {
	[Fact]
	public void Properties_インスタンスのデフォルト値を確認する() {
		// Arrange
		// Act
		var actual = new FeatureDefinition();

		// Assert
		Assert.Null(actual.Name);
		Assert.Empty(actual.EnabledFor);
		Assert.Equal(RequirementType.Any, actual.RequirementType);
		Assert.Equal(FeatureStatus.Conditional, actual.Status);
		Assert.Null(actual.Allocation);
		Assert.Empty(actual.Variants);
		Assert.Null(actual.Telemetry);
	}
}
