using Cisco.Api.Data.Pss;
using FluentAssertions;
using System;
using System.Linq;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
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
		public async void GetCustomerInventoryDetailsRequest_Succeeds()
		{
			var response = await CiscoClient
				.Pss
				.GetCustomerExtendedInventoryDetailsAsync(new CustomerExtendedInventoryDetailsRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				}, CancellationToken.None)
				.ConfigureAwait(false);

			response.Should().BeOfType<CustomerExtendedInventoryDetailsResponse>();
			response.Should().NotBeNull();

			// TODO - property tests
		}
	}
}
