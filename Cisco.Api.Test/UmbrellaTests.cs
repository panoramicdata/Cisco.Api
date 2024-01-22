using Cisco.Api.Data.Umbrella;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class UmbrellaTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/*
	[Fact]
	public async void CreateInternalNetwork_Succeeds()
	{
		// BUG Fix the Umbrella responses, they are not returning the error message.
		// BUG Not honouring the MaxLength attribute on the Name property.
		var response = await CiscoClient
			.Umbrella
			.CreateInternalNetworkAsync(
			new InternalNetworksCreateUpdateRequest
			{
				//Name = "Retail Android Range 5184-S - Corby - Priors Hall Park",
				//Name = "Retail Android Range 5184-S - Corby - Priors Hall…",
				Name = "Retail Android Range 5184-S - Corby - Priors Hall_",
				IpAddress = "10.15.153.64",
				PrefixLength = 26,
				TunnelId = 0
			})
			.ConfigureAwait(true);

		response.Should().BeOfType<InternalNetwork>();
		//response.Should().NotBeEmpty();
		//response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void AddIdentityToPolicy_Succeeds()
	{
		// NotFound: {"error":"Not Found","statusCode":404,"txId":"5dfc02fd-3df1-46d8-b38a-85b992a161b3"}
		var response = await CiscoClient
			.Umbrella
			.AddIdentityToPolicyAsync(
				620254827,
				14355515
			)
			.ConfigureAwait(true);

		// response.Should().BeOfType<int>();
		response.Should<int>();
		//response.Should().HaveCountGreaterThan(0);
	}
	*/

	[Fact]
	public async void ListInternalNetworks_Succeeds()
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
	public async void ListPolicies_Succeeds()
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
