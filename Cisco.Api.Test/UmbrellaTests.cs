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
	public async void CreateInternalNetwork_BadRequestFailed()
	{
		await CiscoClient.Awaiting(ciscoClient =>
			ciscoClient
			.Umbrella
			.CreateInternalNetworkAsync(
				new InternalNetworksCreateUpdateRequest
				{
					Name = "Retail Android Range 5184-S - Corby - Priors Hall ParkRetail Android Range 5184-S - Corby - Priors Hall Park",
					IpAddress = "10.15.153.64",
					PrefixLength = 26,
					TunnelId = 0
				})
		)
			.Should()
			.ThrowAsync<CiscoApiException>()
			.WithMessage("{\"statusCode\":400,\"error\":\"Bad Request\",\"message\":\"child \\\"name\\\" fails because [\\\"name\\\" length must be less than or equal to 50 characters long]\",\"validation\":{\"source\":\"payload\",\"keys\":[\"name\"]}}")
			.ConfigureAwait(true);
	}
	*/

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
	*/

	/*
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

	/*
	// This test shows how the Add/Delete's don't return an int as the docs say.
	// Also, that API limiter is dealt with where MaxAttemptCount is set to 10.
	[Fact]
	public async void AddIdentityToPolicy_Succeeds()
	{
		for (int i = 0; i < 200; i++)
		{

			try
			{
				await CiscoClient
					.Umbrella
					.DeleteIdentityFromPolicyAsync(
						620254827,
						14395110
					)
					.ConfigureAwait(true);
			}
			catch (CiscoApiException ex)
			{
				if (ex.Message.ToLower().Contains("limit"))
				{
					throw;
				}
			}

			try
			{
				await CiscoClient
				.Umbrella
				.AddIdentityToPolicyAsync(
					620254827,
					14395110
				)
				.ConfigureAwait(true);

				await CiscoClient
					.Umbrella
					.DeleteIdentityFromPolicyAsync(
						620254827,
						14395110
					)
					.ConfigureAwait(true);
			}
			catch (CiscoApiException ex)
			{
				if (ex.Message.ToLower().Contains("limit"))
				{
					throw;
				}
			}

		};
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
	public async void MultiQueryCredentialCycling_Succeeds()
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

	/*
	[Fact]
	public async void ListInternalNetworks_FailsWith429()
	{
		// TODO Need to await each call in the loop
		// Loop query a 1000 times to eventually trigger a 429. Catch the exception and assert the message.

		await CiscoClient.Awaiting(async ciscoClient =>
		{
			for (int i = 0; i < 1000; i++)
			{
				await ciscoClient
					.Umbrella
					.ListInternalNetworksAsync()
					.ConfigureAwait(true);
			};

			return ValueTask.CompletedTask;
		}
		)
		.Should()
		.ThrowAsync<CiscoApiException>()
		.WithMessage("429")
		.ConfigureAwait(true);
	}
	*/

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
