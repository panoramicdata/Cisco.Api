using Cisco.Api.Data.Umbrella;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class UmbrellaTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async Task ListInternalNetworks_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task MultiQueryCredentialCycling_Succeeds()
	{
		// Remember to set "DefaultCredentials": "Cae_Umbrella_Fast", which contains 2 client id and secret pairs.

		// Check that token was first
		var response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);

		// Check that token was second
		response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);

		// Check that token was first again
		response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task ListPolicies_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListPoliciesAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Policy>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async Task ListSites_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListSitesAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Site>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}
}
