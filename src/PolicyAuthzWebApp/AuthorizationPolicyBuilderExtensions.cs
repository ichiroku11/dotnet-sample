using Microsoft.AspNetCore.Authorization;

namespace PolicyAuthzWebApp;

public static class AuthorizationPolicyBuilderExtensions {
	public static AuthorizationPolicyBuilder RequireAreaRoles(
		this AuthorizationPolicyBuilder builder, string area, IEnumerable<string> roles) {
		return builder.AddRequirements(new AreaRolesAuthorizationRequirement(area, roles));
	}

	public static AuthorizationPolicyBuilder RequireAreaRoles(
		this AuthorizationPolicyBuilder builder, string area, params string[] roles) {
		return RequireAreaRoles(builder, area, (IEnumerable<string>)roles);
	}
}
