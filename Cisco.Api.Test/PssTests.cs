﻿using Cisco.Api.Data.Pss;
using FluentAssertions;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

[Collection("PssTests")]
public class PssTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void GetCustomersInventoryIdsAsync_Succeeds()
	{
		var response = await CiscoClient
			  .Pss
			  .GetCustomersInventoryIdsAsync(new CustomersInventoryRequest(), CancellationToken.None)
			  .ConfigureAwait(true);

		response.Should().BeOfType<CustomersInventoryResponse>();
		response.Should().NotBeNull();

		response.CustomerInventories.Should().NotBeEmpty();
		response.CustomerInventories.Select(ci => ci.Inventory).Should().NotBeNull();
		response.CustomerInventories.Select(ci => ci.Customer).Should().NotBeNull();

		response.ResponseTimestamp.Should().NotBe(new DateTime());

		response.Message.Should().NotBeNull();
		response.Message.MessageType.Should().NotBeNull();
		response.Message.MessageDetail.Should().NotBeNull();
	}

	[Fact]
	public async void GetCustomerInventoryAsync_OneCustomer_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetCustomersInventoryIdsAsync(
				new CustomersInventoryRequest
				{
					CustomerIds = new List<string> { Config.TestCustomerId }
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.CustomerInventories.Should().HaveCount(1);
	}

	[Fact]
	public async void GetCustomerInventoryDetails_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetCustomerInventoryDetailsAsync(
				new CustomerInventoryDetailsRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<CustomerInventoryDetailsResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetExtendedCustomerInventoryDetails_FirstPage_Succeeds()
	{
		var response = await CiscoClient
			  .Pss
			  .GetCustomerExtendedInventoryDetailsAsync(
					new CustomerExtendedInventoryDetailsRequest
					{
						CustomerId = Config.TestCustomerId,
						InventoryId = Config.TestInventoryId,
						PageStart = 1
					},
					CancellationToken.None)
			  .ConfigureAwait(true);

		response.Should().BeOfType<CustomerExtendedInventoryDetailsResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetAllExtendedDeviceDetail_Succeeds()
	{
		var deviceDetails = await
			GetAllExtendedDeviceDetail(
				CiscoClient,
				Config.TestCustomerId,
				Config.TestInventoryId,
				CancellationToken.None)
			.ConfigureAwait(true);

		deviceDetails.Should().NotBeNullOrEmpty();
	}

	// Example implementation of getting all ExtendedDeviceDetails
	private static async Task<List<ExtendedDeviceDetail>> GetAllExtendedDeviceDetail(
		CiscoClient client,
		string CustomerId,
		string InventoryId,
		CancellationToken cancellationToken)
	{
		var deviceDetails = new List<ExtendedDeviceDetail>();
		var page = 0;
		int pageTotal;
		do
		{
			page++;
			var response = await client
				.Pss
				.GetCustomerExtendedInventoryDetailsAsync(
					new CustomerExtendedInventoryDetailsRequest
					{
						CustomerId = CustomerId,
						InventoryId = InventoryId,
						PageStart = page
					},
					cancellationToken)
				.ConfigureAwait(true);
			deviceDetails.AddRange(response.DeviceDetails);
			pageTotal = response.Pages.PageTotal;
		} while (page < pageTotal);
		return deviceDetails;
	}

	[Fact]
	public async void GetCustomerInventoryPaginatedDetails_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetCustomerInventoryPaginatedDetailsAsync(
				new CustomerInventoryDetailPaginatedRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<CustomerInventoryDetailPaginatedResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void RetrieveAndConvertDeviceConfigs_Succeeds()
	{
		string tempPath = Path.GetTempPath();
		string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
		string outputFilePath = Path.Combine(tempPath, $"device_configs_{timestamp}.zip");

		try
		{
			// Retrieve the zip file
			var memoryStream = await CiscoClient
				.PssConfigs
				.RetrieveDeviceConfigZipAsync(
					new DeviceConfigsRequest
					{
						CustomerId = Config.TestCustomerId,
						DeviceIds = new List<string> { Config.TestDeviceId },
						ConfigType = DeviceConfigsConfigType.Running
					},
					CancellationToken.None)
				.ConfigureAwait(true);

			memoryStream.Should().NotBeNull();

			// Check if memoryStream contains data
			if (memoryStream.Length == 0)
			{
				throw new InvalidOperationException("The memory stream is empty.");
			}

			// Save the zip file to a local path
			using (var fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
			{
				memoryStream.Position = 0;
				await memoryStream.CopyToAsync(fileStream);
			}

			// Verify the file was saved
			File.Exists(outputFilePath).Should().BeTrue();

			// Convert the MemoryStream to the final object
			var deviceConfigResponses = await CiscoClient
				.PssConfigs
				.ExtractDeviceConfigsZipToDictionaryAsync(memoryStream)
				.ConfigureAwait(true);

			deviceConfigResponses.Should().BeOfType<Dictionary<string, DeviceConfigResponse>>();
			deviceConfigResponses.Should().NotBeNull();
			deviceConfigResponses.Should().ContainKey(Config.TestDeviceId);
		}
		catch (Exception ex)
		{
			// Ensure the file is deleted if an exception occurs
			if (File.Exists(outputFilePath))
			{
				File.Delete(outputFilePath);
			}
			throw new Exception("Test failed and the file was deleted.", ex);
		}
		finally
		{
			// Ensure the file is deleted after the test completes
			if (File.Exists(outputFilePath))
			{
				File.Delete(outputFilePath);
			}
		}
	}

	[Fact]
	public async void GetContractCoverageDetailsAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetContractCoverageAsync(
				new ContractCoverageRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractCoverageResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetContractCoverageDetailsOneCardIdAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetContractCoverageAsync(
				new ContractCoverageRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId,
					DeviceIds = new List<string> { Config.TestDeviceId }
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractCoverageResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetContractCoverageDetailsForDeviceIdsAsync_Succeeds()
	{
		// Query upto 100 devices at a time
		var deviceIds = new List<string> { Config.TestDeviceId };

		var response = await CiscoClient
			.Pss
			.GetContractCoverageAsync(
				new ContractCoverageRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId,
					DeviceIds = deviceIds
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<ContractCoverageResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetSoftwareEoxAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetSoftwareEoxAsync(
				new SoftwareEoxRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<SoftwareEoxResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetSoftwareEoxBulletinAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetSoftwareEoxBulletinAsync(
				new SoftwareEoxBulletinRequest
				{
					SoftwareEoxIds = new SoftwareEoxIds
					{
						Ids = new List<string> { Config.TestSoftwareEoxId }
					}
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<SoftwareEoxBulletinResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetHwEoxAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetHardwareEoxAsync(
				new HardwareEoxRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<HardwareEoxResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetHwEoxBulletinAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			// Responses seem to be duplicated per HardwareEoxId
			.GetHardwareEoxBulletinAsync(
				new HardwareEoxBulletinRequest
				{
					HardwareEoxIds = new HardwareEoxIds
					{
						Ids = new List<string> { Config.TestHardwareEoxId }
					}
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<HardwareEoxBulletinResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetPsirtAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetPsirtAsync(
				new PsirtRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<PsirtResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetPsirtDetailsAsync_Succeeds()
	{
		var ids = new List<string> { Config.TestPsirtId1, Config.TestPsirtId2 };

		var response = await CiscoClient
			.Pss
			.GetPsirtDetailsAsync(
				new PsirtDetailsRequest
				{
					Ids = ids
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<PsirtDetailsResponse>();
		response.Should().NotBeNull();
		response.Details.Count.Should().Be(ids.Count);
		// TODO - property tests
	}

	[Fact]
	public async void GetFieldNoticesAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetFieldNoticesAsync(
				new FieldNoticesRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<FieldNoticesResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}

	[Fact]
	public async void GetFieldNoticesDetailsAsync_Succeeds()
	{
		var response = await CiscoClient
			.Pss
			.GetFieldNoticesDetailsAsync(
				new FieldNoticesDetailsRequest
				{
					Ids = new List<string>
						{
							Config.TestFieldNoticesId1,
							Config.TestFieldNoticesId2
						}
				},
				CancellationToken.None)
			.ConfigureAwait(true);

		response.Should().BeOfType<FieldNoticesDetailsResponse>();
		response.Should().NotBeNull();

		// TODO - property tests
	}
}
