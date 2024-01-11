using Cisco.Api.Data.Umbrella;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class UmbrellaTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetAllInternalNetworks_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.GetAllInternalNetworksAsync(default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<InternalNetwork>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void GetAllPolicies_Succeeds()
	{
		var response = await CiscoClient
			.Umbrella
			.GetAllPoliciesAsync(default)
			.ConfigureAwait(true);

		response.Should().BeOfType<List<Policy>>();
		response.Should().NotBeEmpty();
		response.Should().HaveCountGreaterThan(0);
	}
}
