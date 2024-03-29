using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PolicyAuthzWebApp;

// エリアにアクセスできるロールを指定する要件
// 作ってはみたものの結局はエリア専用のコントローラを作るか、エンドポイントで指定するかって気がしてきた
/*
[Area("Hoge"), Authorize(Roles = "Fuge")]
public abstract class HogeAreaController : Controller {
}
*/
public class AreaRolesAuthorizationRequirement(string area, IEnumerable<string> allowedRoles)
	: AuthorizationHandler<AreaRolesAuthorizationRequirement>, IAuthorizationRequirement {
	public string Area { get; } = area;
	public IEnumerable<string> AllowedRoles { get; } = allowedRoles;
	public RolesAuthorizationRequirement RolesAuthorizationRequirement { get; } = new RolesAuthorizationRequirement(allowedRoles);

	private string GetAreaName(object? resource) {
		if (resource is AuthorizationFilterContext context) {
			return context.HttpContext.Request.RouteValues["area"] as string ?? "";
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
