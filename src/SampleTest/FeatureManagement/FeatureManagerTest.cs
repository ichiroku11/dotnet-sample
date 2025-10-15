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

		public FeatureDefinitionProvider(params IEnumerable<FeatureDefinition> definitions) {
			foreach (var definition in definitions) {
				_definitions[definition.Name] = definition;
			}
		}

		public FeatureDefinitionProvider(params IEnumerable<string> names)
			: this(names.Select(name => new FeatureDefinition { Name = name })) {
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

	[Fact]
	public async Task IsEnabledAsync_名前だけを設定したFeatureDefinitionではfalseになる() {
		// Arrange
		var featureNames = new List<string> { "feature" };
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureNames);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = await featureManager.IsEnabledAsync("feature");

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async Task IsEnabledAsync_EnableForがnullのFeatureDefinitionではfalseになる() {
		// Arrange
		var featureDefinition = new FeatureDefinition {
			Name = "feature",
			EnabledFor = null,
		};
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureDefinition);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = await featureManager.IsEnabledAsync("feature");

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async Task IsEnabledAsync_EnableForが空のFeatureDefinitionではfalseになる() {
		// Arrange
		var featureDefinition = new FeatureDefinition {
			Name = "feature",
			EnabledFor = [],
		};
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureDefinition);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = await featureManager.IsEnabledAsync("feature");

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async Task IsEnabledAsync_FeatureStatusがDisabledのFeatureDefinitionではfalseになる() {
		// Arrange
		var featureDefinition = new FeatureDefinition {
			Name = "feature",
			// EnabledForがnullでも空でもない
			EnabledFor = [new FeatureFilterConfiguration()],
			Status = FeatureStatus.Disabled,
		};
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureDefinition);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var actual = await featureManager.IsEnabledAsync("feature");

		// Assert
		Assert.False(actual);
	}

	[Fact]
	public async Task IsEnabledAsync_該当するフィルターが存在しないとFeatureManagementExceptionが発生する() {
		// Arrange
		var featureDefinition = new FeatureDefinition {
			Name = "feature",
			EnabledFor = [new FeatureFilterConfiguration()],
			// デフォルト
			//Status = FeatureStatus.Conditional,
		};
		var featureDefinitionProvider = new FeatureDefinitionProvider(featureDefinition);
		var featureManager = new FeatureManager(featureDefinitionProvider);

		// Act
		var exception = await Record.ExceptionAsync(async () => {
			await featureManager.IsEnabledAsync("feature");
		});

		// Assert
		Assert.IsType<FeatureManagementException>(exception);
		_output.WriteLine(exception.Message);
		// The feature filter '' specified for feature 'feature' was not found.
	}
}
