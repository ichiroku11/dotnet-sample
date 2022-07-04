using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagWebApp.Controllers;
public class HomeController : Controller {
	// https://docs.microsoft.com/ja-jp/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x
	private readonly IFeatureManager _featureManager;

	public HomeController(IFeatureManager featureManager) {
		_featureManager = featureManager;
	}

	// 機能フラグの確認
	public async Task<IActionResult> IndexAsync()
		=> Json(new {
			featureA = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureA),
			featureB = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureB),
			featureC = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureC),
		});

	// 機能フラグによるフィルター
	// 機能フラグが無効の場合、既定では404
	[FeatureGate(FeatureFlags.FeatureA)]
	public IActionResult FeatureA() => Content(nameof(FeatureA));

	[FeatureGate(FeatureFlags.FeatureB)]
	public IActionResult FeatureB() => Content(nameof(FeatureB));

	[FeatureGate(FeatureFlags.FeatureC)]
	public IActionResult FeatureC() => Content(nameof(FeatureC));
}
