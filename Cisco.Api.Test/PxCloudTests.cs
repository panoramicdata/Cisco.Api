using Cisco.Api.Data.PxCloud;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for PX cloud operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class PxCloudTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the get customers succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetCustomers_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetCustomersAsync(0, 10, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<Customers>();
	}

	/// <summary>
	/// Verifies the get all customers succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetAllCustomers_Succeeds()
	{
		// Offset will store the number of customers retrieved so far. Call GetCustomersAsync with 'max' set to 50, to get the first 50 items, and the total count
		// Keep calling GetCustomersAsync with 'offset' set to the current count, until the total count is reached
		var offset = 0;
		var max = 50;
		var customers = new List<Customer>();
		var totalCount = -1;
		Customers response;

		while (totalCount == -1 || offset < totalCount)
		{
			response = await CiscoClient
				.PxCloud
				.GetCustomersAsync(offset, max, default)
				.ConfigureAwait(true);

			if (totalCount == -1)
			{
				totalCount = response.TotalCount;
			}

			response.Items.Should().NotBeEmpty();

			customers.AddRange(response.Items);
			offset += response.Items.Count;
		}

		customers.Should().NotBeEmpty();
	}

	/// <summary>
	/// Verifies the get contracts succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetContracts_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractsAsync(0, 10, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<Contracts>();
	}

	/// <summary>
	/// Verifies the get contracts with customers succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetContractsWithCustomers_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractsWithCustomersAsync(0, 10, null, null, null, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractsWithCustomers>();
	}

	/// <summary>
	/// Verifies the get contract details succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetContractDetails_Succeeds()
	{
		var response = await CiscoClient
			.PxCloud
			.GetContractDetailsAsync("205241272", 0, 10, null, null, null, default)
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractDetails>();
	}

	/// <summary>
	/// Verifies the get all contract details succeeds scenario.
	/// </summary>
	[Fact]
	public async Task GetAllContractDetails_Succeeds()
	{
		// Offset will store the number of customers retrieved so far. Call GetCustomersAsync with 'max' set to 50, to get the first 50 items, and the total count
		// Keep calling GetCustomersAsync with 'offset' set to the current count, until the total count is reached
		var offset = 0;
		var max = 50;
		var contractDetails = new List<ContractDetail>();
		var totalCount = -1;
		ContractDetails response;

		while (totalCount == -1 || offset < totalCount)
		{
			response = await CiscoClient
				.PxCloud
				.GetContractDetailsAsync("205241272", offset, max, null, null, null, default)
				.ConfigureAwait(true);

			if (totalCount == -1)
			{
				totalCount = response.TotalCount;
			}

			response.Items.Should().NotBeEmpty();

			contractDetails.AddRange(response.Items);
			offset += response.Items.Count;
		}

		contractDetails.Should().NotBeEmpty();
	}

	/// <summary>
	/// Verifies the request assets report succeeds scenario.
	/// </summary>
	[Fact]
	public async Task RequestAssetsReport_Succeeds()
	{
		// KCL has all reports except for PriorityBugs

		// Get the report ID
		var response = await CiscoClient
			.PxCloudReports
			.RequestCustomerDataReportAsync("ojD5nF68Lrip1oE", ReportName.Assets, "38396885", default)
			.ConfigureAwait(true);

		response.Should().BeOfType<RequestCustomerDataReportsAsBulkFilesResponse>();

		var reportId = response.ReportId;

		// Get the report
		var report = await CiscoClient
			.PxCloudReports
			.GetReportAsync("ojD5nF68Lrip1oE", reportId, default)
			.ConfigureAwait(true);

		report.Should().NotBeNull();
	}
}
