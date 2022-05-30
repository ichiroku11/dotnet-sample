using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAdB2cMsalConsoleApp;

public class InMemoryTokenCache {
	public void Bind(ITokenCache tokenCache) {
		tokenCache.SetBeforeAccessAsync(OnBeforeAccessAsync);
		tokenCache.SetBeforeWriteAsync(OnBeforeWriteAsync);
		tokenCache.SetAfterAccessAsync(OnAfterAccessAsync);
	}

	private Task OnBeforeAccessAsync(TokenCacheNotificationArgs arg) {
		// todo:
		return Task.CompletedTask;
	}

	private Task OnBeforeWriteAsync(TokenCacheNotificationArgs arg) {
		// todo:
		return Task.CompletedTask;
	}

	private Task OnAfterAccessAsync(TokenCacheNotificationArgs arg) {
		// todo:
		return Task.CompletedTask;
	}
}
