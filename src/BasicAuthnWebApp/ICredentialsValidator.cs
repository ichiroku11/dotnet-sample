using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BasicAuthnWebApp {
	public interface ICredentialsValidator {
		// 資格情報を検証する
		Task<ClaimsPrincipal> ValidateAsync(string userName, string password, AuthenticationScheme scheme);
	}
}
