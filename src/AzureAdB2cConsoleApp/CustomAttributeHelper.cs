using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdB2cConsoleApp {
	public class CustomAttributeHelper {
		private readonly string _extensionAppClientId;

		public CustomAttributeHelper(string extensionAppClientId) {
			_extensionAppClientId = extensionAppClientId.Replace("-", "");
		}

		public string GetFullName(string attributeName) => $"extension_{_extensionAppClientId}_{attributeName}";
	}
}
