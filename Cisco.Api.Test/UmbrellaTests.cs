using Cisco.Api.Data.Umbrella;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for umbrella operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class UmbrellaTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the list internal networks succeeds scenario.
	/// </summary>
	[Fact]
	public async Task ListInternalNetworks_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync(null, 1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	/// <summary>
	/// Verifies the multi query credential cycling succeeds scenario.
	/// </summary>
	[Fact]
	public async Task MultiQueryCredentialCycling_Succeeds()
	{
		// Remember to set "DefaultCredentials": "Cae_Umbrella_Fast", which contains 2 client id and secret pairs.

		// Check that token was first
		var response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync(null, 1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);

		// Check that token was second
		response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync(null, 1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);

		// Check that token was first again
		response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync(null, 1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	/// <summary>
	/// Verifies the list policies succeeds scenario.
	/// </summary>
	[Fact]
	public async Task ListPolicies_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListPoliciesAsync("dns", 1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Policy>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	/// <summary>
	/// Verifies the list sites succeeds scenario.
	/// </summary>
	[Fact]
	public async Task ListSites_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListSitesAsync(1, 100, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Site>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}
}
