using System.Security.Claims;

namespace SampleTest;

/// <summary>
/// 
/// </summary>
public static class AssertHelper {
	/// <summary>
	/// 
	/// </summary>
	/// <param name="claims"></param>
	/// <param name="type"></param>
	/// <param name="value"></param>
	/// <returns></returns>
	public static Claim ContainsClaim(IEnumerable<Claim> claims, string type, string value)
		=> Assert.Single(
			claims,
			claim =>
				string.Equals(claim.Type, type, StringComparison.Ordinal) &&
				string.Equals(claim.Value, value, StringComparison.Ordinal));

	/// <summary>
	/// 
	/// </summary>
	/// <param name="claims"></param>
	/// <param name="type"></param>
	/// <param name="value"></param>
	/// <param name="valueType"></param>
	/// <returns></returns>
	public static Claim ContainsClaim(IEnumerable<Claim> claims, string type, string value, string valueType)
		=> Assert.Single(
			claims,
			claim =>
				string.Equals(claim.Type, type, StringComparison.Ordinal) &&
				string.Equals(claim.Value, value, StringComparison.Ordinal) &&
				string.Equals(claim.ValueType, valueType, StringComparison.Ordinal));
}
