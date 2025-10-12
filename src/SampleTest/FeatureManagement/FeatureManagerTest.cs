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

	// テスト用
	private class FeatureDefinitionProvider : IFeatureDefinitionProvider {
		private static readonly Dictionary<string, FeatureDefinition> _definitions = [];

		public FeatureDefinitionProvider(params IEnumerable<string> featureNames) {
			foreach (var featureName in featureNames) {
				_definitions[featureName] = new FeatureDefinition { Name = featureName };
			}
		}

		public async IAsyncEnumerable<FeatureDefinition> GetAllFeatureDefinitionsAsync() {
			foreach (var definition in _definitions.Values) {
				yield return definition;
				await Task.Yield();
			}
		}

		public Task<FeatureDefinition> GetFeatureDefinitionAsync(string featureName)
			=> Task.FromResult(_definitions[featureName]);
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

	[Fact]
	public async Task GetFeatureNamesAsync_機能名を取得できる() {
		// Arrange
		var expected = new List<string> { "feature1", "feature2" };
		var featureDefinitionProvider = new FeatureDefinitionProvider(expected);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = new List<string>();
		await foreach (var name in featureManager.GetFeatureNamesAsync()) {
			actual.Add(name);
		}

		// Assert
		Assert.Equal(expected, actual);
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

	[Theory]
	[InlineData("feature1", false)]
	public async Task IsEnabledAsync_名前だけを設定したFeatureDefinitionではfalseになる(string? featureName, bool expected) {
		// Arrange
		var featureNames = new List<string> { "feature1" };
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureNames);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = await featureManager.IsEnabledAsync(featureName);

		// Assert
		Assert.Equal(expected, actual);
	}
}
