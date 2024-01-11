using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OptionPatternWebApp.Controllers;

public class DefaultController(IOptions<SampleOptions> options, IOptionsMonitor<SampleOptionsMonitor> optionsMonitor) : Controller {
	private readonly SampleOptions _options = options.Value;
	// アプリケーション実行中の変更が反映される設定
	private readonly SampleOptionsMonitor _optionsMonitor = optionsMonitor.CurrentValue;

	public IActionResult Index() {
		var content
			= nameof(SampleOptions)
			+ Environment.NewLine
			+ $"{nameof(SampleOptions.Value1)}: {_options.Value1}"
			+ Environment.NewLine
			+ $"{nameof(SampleOptions.Value2)}: {_options.Value2}"
			+ Environment.NewLine
			+ nameof(SampleOptionsMonitor)
			+ Environment.NewLine
			+ $"{nameof(SampleOptionsMonitor.Value1)}: {_optionsMonitor.Value1}"
			+ Environment.NewLine
			+ $"{nameof(SampleOptionsMonitor.Value2)}: {_optionsMonitor.Value2}";

		return Content(content);
	}
}
