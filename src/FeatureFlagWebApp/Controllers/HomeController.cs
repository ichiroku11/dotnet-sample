using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace FeatureFlagWebApp.Controllers;
public class HomeController : Controller {
	private readonly IFeatureManager _featureManager;

	public HomeController(IFeatureManager featureManager) {
		_featureManager = featureManager;
	}

	public async Task<IActionResult> IndexAsync()
		=> Json(new {
			featureA = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureA),
			featureB = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureB),
			featureC = await _featureManager.IsEnabledAsync(FeatureFlags.FeatureC),
		});
}
