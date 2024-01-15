using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagWebApp.Controllers;
public class HomeController(IFeatureManager featureManager) : Controller {
	// https://docs.microsoft.com/ja-jp/azure/azure-app-configuration/use-feature-flags-dotnet-core?tabs=core5x
	private readonly IFeatureManager _featureManager = featureManager;

	public IActionResult Index() => View();

	// 機能フラグの確認
	public async Task<IActionResult> JsonAsync()
		=> Json(new {
			featureA = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureA),
			featureB = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureB),
			featureC = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureC),
		});


	// 機能フラグによるフィルター
	// 機能フラグが無効の場合、既定では404
	[Route("[controller]/feature/a")]
	[FeatureGate(FeatureFlags.FeatureA)]
	public IActionResult FeatureA() => Content(nameof(FeatureA));

	[Route("[controller]/feature/b")]
	[FeatureGate(FeatureFlags.FeatureB)]
	public IActionResult FeatureB() => Content(nameof(FeatureB));

	[Route("[controller]/feature/c")]
	[FeatureGate(FeatureFlags.FeatureC)]
	public IActionResult FeatureC() => Content(nameof(FeatureC));

	// AとBどちらも設定されている必要がある
	[Route("[controller]/feature/andb")]
	[FeatureGate(RequirementType.All, FeatureFlags.FeatureA, FeatureFlags.FeatureB)]
	public IActionResult FeatureAAndB() => Content($"{nameof(FeatureA)} and {nameof(FeatureB)}");

	// AとBどちらかが設定されていればOK
	[Route("[controller]/feature/aorb")]
	[FeatureGate(RequirementType.Any, FeatureFlags.FeatureA, FeatureFlags.FeatureB)]
	public IActionResult FeatureAOrB() => Content($"{nameof(FeatureA)} or {nameof(FeatureB)}");
}
