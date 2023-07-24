using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cisco.Api.Data.Pss;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	[Collection("PssTests")]
	public class PssTests : Test
	{
		public PssTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task GetCustomersInventoryIdsAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetCustomersInventoryIdsAsync(new CustomersInventoryRequest(), CancellationToken.None)
				  .ConfigureAwait(false);

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
		public async Task GetCustomerInventoryAsync_OneCustomer_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetCustomersInventoryIdsAsync(new CustomersInventoryRequest
				  {
					  CustomerIds = new List<string>
						 {
										  Config.TestCustomerId
						 }
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.CustomerInventories.Should().HaveCount(1);
		}

		[Fact]
		public async Task GetCustomerInventoryDetails_Succeeds()
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
				  .ConfigureAwait(false);

			response.Should().BeOfType<CustomerInventoryDetailsResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetExtendedCustomerInventoryDetails_FirstPage_Succeeds()
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
				  .ConfigureAwait(false);

			response.Should().BeOfType<CustomerExtendedInventoryDetailsResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetAllExtendedDeviceDetail_Succeeds()
		{
			var deviceDetails = await GetAllExtendedDeviceDetail(
				CiscoClient,
				Config.TestCustomerId,
				Config.TestInventoryId,
				CancellationToken.None)
				  .ConfigureAwait(false);

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
					  .ConfigureAwait(false);
				deviceDetails.AddRange(response.DeviceDetails);
				pageTotal = response.Pages.PageTotal;
			} while (page < pageTotal);
			return deviceDetails;
		}

		[Fact]
		public async Task GetCustomerInventoryPaginatedDetails_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetCustomerInventoryPaginatedDetailsAsync(new CustomerInventoryDetailPaginatedRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<CustomerInventoryDetailPaginatedResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetContractCoverageDetailsAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetContractCoverageAsync(new ContractCoverageRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<ContractCoverageResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetContractCoverageDetailsOneCardIdAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetContractCoverageAsync(new ContractCoverageRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId,
					  DeviceIds = new List<string> { Config.TestDeviceId }
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<ContractCoverageResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetSoftwareEoxAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetSoftwareEoxAsync(new SoftwareEoxRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<SoftwareEoxResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetSoftwareEoxBulletinAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetSoftwareEoxBulletinAsync(new SoftwareEoxBulletinRequest
				  {
					  SoftwareEoxIds = new SoftwareEoxIds
					  {
						  Ids = new List<string>
									  {
												 Config.TestSoftwareEoxId
									  }
					  }
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<SoftwareEoxBulletinResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetHwEoxAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetHardwareEoxAsync(new HardwareEoxRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<HardwareEoxResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetHwEoxBulletinAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  // Responses seem to be duplicated per HardwareEoxId
				  .GetHardwareEoxBulletinAsync(new HardwareEoxBulletinRequest
				  {
					  HardwareEoxIds = new HardwareEoxIds
					  {
						  Ids = new List<string>
								 {
												 Config.TestHardwareEoxId
								 }
					  }
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<HardwareEoxBulletinResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetPsirtAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetPsirtAsync(new PsirtRequest
				  {
					  CustomerId = "PSS_3151974", // Config.TestCustomerId,
					  InventoryId = "3232417", // Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<PsirtResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetPsirtDetailsAsync_Succeeds()
		{
			var ids = new List<string>
				{
							Config.TestPsirtId1,
							Config.TestPsirtId2
				};

			var response = await CiscoClient
				  .Pss
				  .GetPsirtDetailsAsync(new PsirtDetailsRequest
				  {
					  Ids = ids
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<PsirtDetailsResponse>();
			response.Should().NotBeNull();
			response.Details.Count.Should().Be(ids.Count);
			// TODO - property tests
		}

		[Fact]
		public async Task GetFieldNoticesAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetFieldNoticesAsync(new FieldNoticesRequest
				  {
					  CustomerId = Config.TestCustomerId,
					  InventoryId = Config.TestInventoryId
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<FieldNoticesResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async Task GetFieldNoticesDetailsAsync_Succeeds()
		{
			var response = await CiscoClient
				  .Pss
				  .GetFieldNoticesDetailsAsync(new FieldNoticesDetailsRequest
				  {
					  Ids = new List<string>
							{
									 Config.TestFieldNoticesId1,
									 Config.TestFieldNoticesId2
							}
				  }, CancellationToken.None)
				  .ConfigureAwait(false);

			response.Should().BeOfType<FieldNoticesDetailsResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}
	}
}
