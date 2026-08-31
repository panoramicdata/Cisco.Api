using Cisco.Api.Data.Psirt;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for PSIRT operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class PsirtTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the get PSIRT by CVE ID scenario.
	/// </summary>
	[Fact]
	public async Task GetPsirtByCveId()
	{
		var advisoryResponse = await CiscoClient
			.Psirt
			.GetByCveIdAsync("CVE-2020-3433", CancellationToken.None)
			.ConfigureAwait(true);

		advisoryResponse.Should().NotBeNull();
		advisoryResponse.Should().BeOfType<AdvisoriesResponse>();
		advisoryResponse.Advisories.Should().NotBeNull();
		advisoryResponse.Advisories.Should().NotBeEmpty();
	}

	/// <summary>
	/// Verifies the get all psirts scenario.
	/// </summary>
	[Fact]
	public async Task GetAllPsirts()
	{
		var advisoryResponse = await CiscoClient
			.Psirt
			.GetAllAsync(CancellationToken.None)
			.ConfigureAwait(true);

		advisoryResponse.Should().NotBeNull();
		advisoryResponse.Should().BeOfType<AdvisoriesResponse>();
		advisoryResponse.Advisories.Should().NotBeNull();
		advisoryResponse.Advisories.Should().NotBeEmpty();
		advisoryResponse.Advisories.Count.Should().BeGreaterThan(100);
	}
}