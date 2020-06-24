using Cisco.Api.Data.Pss;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
		public async void GetCustomerInventoryAsync_Succeeds()
		{
			var response = await CiscoClient
				.Pss
				.GetCustomerInventoryAsync(new CustomersInventoryRequest(), CancellationToken.None)
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
		public async void GetCustomerInventoryAsync_OneCustomer_Succeeds()
		{
			var response = await CiscoClient
				.Pss
				.GetCustomerInventoryAsync(new CustomersInventoryRequest
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
		public async void GetCustomerInventoryPaginatedDetails_Succeeds()
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
		public async void GetContractCoverageDetailsAsync_Succeeds()
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
		public async void GetSoftwareEoxAsync_Succeeds()
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
		public async void GetSoftwareEoxBulletinAsync_Succeeds()
		{
			var response = await CiscoClient
				.Pss
				.GetSoftwareEoxBulletinAsync(new SoftwareEoxBulletinRequest
				{
					SoftwareEoxIds = new List<SoftwareEoxId>
					{
						new SoftwareEoxId()
						{
							Id = Config.TestSoftwareEoxId
						}
					}
				}, CancellationToken.None)
				.ConfigureAwait(false);

			response.Should().BeOfType<SoftwareEoxBulletinResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}

		[Fact]
		public async void GetHwEoxAsync_Succeeds()
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
	}
}
