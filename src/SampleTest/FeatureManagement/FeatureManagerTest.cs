using Microsoft.FeatureManagement;

namespace SampleTest.FeatureManagement;

public class FeatureManagerTest(ITestOutputHelper output) {
	private readonly ITestOutputHelper _output = output;

	// 常にNotImplementedExceptionをスローする
	private class NotImplementedFeatureDefinitionProvider : IFeatureDefinitionProvider {
		public const string MessageOfGetAll = nameof(GetAllFeatureDefinitionsAsync);
		public const string MessageOfGet = nameof(GetFeatureDefinitionAsync);

		public IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync()
			=> throw new NotImplementedException(message: MessageOfGetAll);

		public Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
			=> throw new NotImplementedException(message: MessageOfGet);
	}

	[Fact]
	public async Task GetFeatureNamesAsync_FeatureDefinitionProviderのGetAllFeatureDefinitionsAsyncが呼ばれることを確認する() {
		// Arrange
		var featureManager = new FeatureManager(new NotImplementedFeatureDefinitionProvider());

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await foreach (var name in featureManager.GetFeatureNamesAsync()) {
			}
		});

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<NotImplementedException>(exception);
		Assert.Equal(NotImplementedFeatureDefinitionProvider.MessageOfGetAll, exception.Message);
		_output.WriteLine(exception.Message);
	}

	[Theory]
	[InlineData(null)]
	[InlineData("")]
	[InlineData("notfound")]
	public async Task IsEnabledAsync_FeatureDefinitionProviderのGetFeatureDefinitionAsyncが呼ばれることを確認する(string? feature) {
		// Arrange
		var featureManager = new FeatureManager(new NotImplementedFeatureDefinitionProvider());

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await featureManager.IsEnabledAsync(feature);
		});

		// Assert
		Assert.NotNull(exception);
		Assert.IsType<NotImplementedException>(exception);
		Assert.Equal(NotImplementedFeatureDefinitionProvider.MessageOfGet, exception.Message);
		_output.WriteLine(exception.Message);
	}
}
