using System.Security.Claims;

namespace MiscWebApi.Models;

public static class ClaimsPrincipalExtensions {
	public static UserMeResponse ToUserMeResponse(this ClaimsPrincipal principal) {
		var id = principal.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
		var name = principal.Claims.First(claim => claim.Type == ClaimTypes.Name).Value;

		return new UserMeResponse(id, name);
	}
}
