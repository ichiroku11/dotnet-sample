using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace SampleLib.AspNetCore.Authentication.Basic;

public interface ICredentialsValidator {
	// 資格情報を検証する
	Task<ClaimsPrincipal?> ValidateAsync(string userName, string password, AuthenticationScheme scheme);
}
