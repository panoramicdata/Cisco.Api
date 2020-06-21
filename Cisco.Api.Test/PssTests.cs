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
		public async void GetCustomerInventoryDetailsRequest_Succeeds()
		{
			var customerInventoryResponse = await CiscoClient
				.Pss
				.GetCustomerInventoryAsync(new CustomersInventoryRequest(), CancellationToken.None)
				.ConfigureAwait(false);

			customerInventoryResponse.Should().NotBeNull();

			customerInventoryResponse.CustomerInventories.Should().NotBeEmpty();
			customerInventoryResponse.CustomerInventories.Select(ci => ci.Inventory).Should().NotBeNull();
			customerInventoryResponse.CustomerInventories.Select(ci => ci.Customer).Should().NotBeNull();

			customerInventoryResponse.ResponseTimestamp.Should().NotBe(new DateTime());

			customerInventoryResponse.Message.Should().NotBeNull();
			customerInventoryResponse.Message.MessageType.Should().NotBeNull();
			customerInventoryResponse.Message.MessageDetail.Should().NotBeNull();
		}
	}
}
