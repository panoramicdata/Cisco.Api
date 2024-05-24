using Cisco.Api.Data.PxCloud;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class PxCloudTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetCustomers_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetCustomersAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<Customers>();
		//response.Should().NotBeEmpty();
		//response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void GetContracts_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractsAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<Contracts>();
		//response.Should().NotBeEmpty();
		//response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void GetContractsWithCustomers_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractsWithCustomersAsync()
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractsWithCustomers>();
		//response.Should().NotBeEmpty();
		//response.Should().HaveCountGreaterThan(0);
	}

	[Fact]
	public async void GetContractDetails_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractDetailsAsync("205241272")
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractDetails>();
		//response.Should().NotBeEmpty();
		//response.Should().HaveCountGreaterThan(0);
	}
}
