using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyAuthzWebApp {
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
}
