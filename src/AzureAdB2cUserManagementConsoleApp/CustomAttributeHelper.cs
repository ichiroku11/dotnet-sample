namespace AzureAdB2cUserManagementConsoleApp;

public class CustomAttributeHelper(string extensionAppClientId) {
	private readonly string _extensionAppClientId = extensionAppClientId.Replace("-", "");

	public string GetFullName(string attributeName) => $"extension_{_extensionAppClientId}_{attributeName}";
}
