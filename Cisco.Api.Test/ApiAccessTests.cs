using Cisco.Api;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Exercises the ApiAccess type's Any aggregator.
/// </summary>
public class ApiAccessTests
{
	/// <summary>Any is true when at least one API is reachable.</summary>
	[Fact]
	public void Any_IsTrue_WhenAnyFlagIsSet()
	{
		var access = new ApiAccess
		{
			Eox = true,
			Umbrella = true
		};
		access.Any.Should().BeTrue();
		access.EnterpriseAgreement.Should().BeFalse();
	}

	/// <summary>Any is false when no flag is set.</summary>
	[Fact]
	public void Any_IsFalse_WhenNoFlagIsSet()
	{
		new ApiAccess().Any.Should().BeFalse();
	}
}
