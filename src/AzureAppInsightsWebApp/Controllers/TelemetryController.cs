using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;

namespace AzureAppInsightsWebApp.Controllers;

// カスタムイベントやメトリックを収集する
// https://docs.microsoft.com/ja-jp/azure/azure-monitor/app/api-custom-events-metrics#api-summary
public class TelemetryController : Controller {
	private readonly TelemetryClient _client;

	public TelemetryController(TelemetryClient client) {
		_client = client;
	}

	public IActionResult Event() {
		var name = nameof(Event).ToLower();

		var properties = new Dictionary<string, string>() {
			[$"{name}.key1"] = $"{name}.value1",
			[$"{name}.key2"] = $"{name}.value2",
		};
		_client.TrackEvent("Sample", properties);

		return Content(name);
	}
}
