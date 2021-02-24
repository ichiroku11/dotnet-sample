using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace OptionPatternWebApp.Controllers {
	public class DefaultController : Controller {
		private readonly SampleOptions _options;
		// アプリケーション実行中の変更が反映される設定
		private readonly SampleOptionsMonitor _optionsMonitor;

		public DefaultController(IOptions<SampleOptions> options, IOptionsMonitor<SampleOptionsMonitor> optionsMonitor) {
			_options = options.Value;
			_optionsMonitor = optionsMonitor.CurrentValue;
		}

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
}
