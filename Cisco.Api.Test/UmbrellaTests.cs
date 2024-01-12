using Cisco.Api.Data.Umbrella;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class UmbrellaTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void ListInternalNetworks_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListInternalNetworksAsync(default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void ListPolicies_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.ListPoliciesAsync("web")
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Policy>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void ListSites_Succeeds()
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
