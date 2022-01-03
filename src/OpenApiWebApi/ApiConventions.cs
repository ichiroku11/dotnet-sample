using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace OpenApiWebApi;

// 参考
// https://github.com/dotnet/aspnetcore/blob/master/src/Mvc/Mvc.Core/src/DefaultApiConventions.cs
/// <summary>
/// 
/// </summary>
public static class ApiConventions {
	/// <summary>
	/// Get
	/// </summary>
	/// <param name="id"></param>
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
	public static void Get(
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)]
			[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
			object id) {
	}

	/// <summary>
	/// Post
	/// </summary>
	/// <param name="model"></param>
	[ProducesResponseType(StatusCodes.Status201Created)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesDefaultResponseType]
	[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
	public static void Post(
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
			[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
			object model) {
	}

	/// <summary>
	/// Put
	/// </summary>
	/// <param name="id"></param>
	/// <param name="model"></param>
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Prefix)]
	public static void Put(
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)]
			[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
			object id,
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Any)]
			[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
			object model) {
	}

	/// <summary>
	/// Delete
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	[ProducesResponseType(StatusCodes.Status204NoContent)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesDefaultResponseType]
	public static void Delete(
		[ApiConventionNameMatch(ApiConventionNameMatchBehavior.Suffix)]
			[ApiConventionTypeMatch(ApiConventionTypeMatchBehavior.Any)]
			object id) {
	}
}
