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
		var advisoryResponse = await GetByCveIdAsync("CVE-2020-3433");

		AssertAdvisories(advisoryResponse);
	}

	/// <summary>
	/// Verifies the get all psirts scenario.
	/// </summary>
	[Fact]
	public async Task GetAllPsirts()
	{
		var advisoryResponse = await GetAllAsync();

		AssertAdvisories(advisoryResponse);
		advisoryResponse.Advisories.Count.Should().BeGreaterThan(100);
	}

	private async Task<AdvisoriesResponse> GetByCveIdAsync(string cveId)
	{
		var response = await CiscoClient
			.Psirt
			.GetByCveIdAsync(cveId, CancellationToken.None)
			.ConfigureAwait(true);
		return response;
	}

	private async Task<AdvisoriesResponse> GetAllAsync()
	{
		var response = await CiscoClient
			.Psirt
			.GetAllAsync(CancellationToken.None)
			.ConfigureAwait(true);
		return response;
	}

	private static void AssertAdvisories(AdvisoriesResponse advisoryResponse)
	{
		advisoryResponse.Should().NotBeNull();
		advisoryResponse.Should().BeOfType<AdvisoriesResponse>();
		advisoryResponse.Advisories.Should().NotBeNull();
		advisoryResponse.Advisories.Should().NotBeEmpty();
	}
}