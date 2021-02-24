using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyAuthzWebApp {
	// エリアにアクセスできるロールを指定する要件
	// 作ってはみたものの結局はエリア専用のコントローラを作るか、エンドポイントで指定するかって気がしてきた
	/*
	[Area("Hoge"), Authorize(Roles = "Fuge")]
	public abstract class HogeAreaController : Controller {
	}
	*/
	public class AreaRolesAuthorizationRequirement : AuthorizationHandler<AreaRolesAuthorizationRequirement>, IAuthorizationRequirement {
		public AreaRolesAuthorizationRequirement(string area, IEnumerable<string> allowedRoles) {
			Area = area;
			AllowedRoles = allowedRoles;
			RolesAuthorizationRequirement = new RolesAuthorizationRequirement(allowedRoles);
		}

		public string Area { get; }
		public IEnumerable<string> AllowedRoles { get; }
		public RolesAuthorizationRequirement RolesAuthorizationRequirement { get; }

		private string GetAreaName(object resource) {
			if (resource is AuthorizationFilterContext context) {
				return context.HttpContext.Request.RouteValues["area"] as string;
			}

			throw new InvalidOperationException();
		}

		protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AreaRolesAuthorizationRequirement requirement) {
			var area = GetAreaName(context.Resource);

			if (string.IsNullOrEmpty(area)) {
				return Task.CompletedTask;
			}

			if (!string.Equals(area, requirement.Area, StringComparison.OrdinalIgnoreCase)) {
				return Task.CompletedTask;
			}

			return requirement.RolesAuthorizationRequirement.HandleAsync(context);
		}
	}
}
