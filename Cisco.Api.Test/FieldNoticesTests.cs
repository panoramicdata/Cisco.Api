using Cisco.Api.Data.Pss;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class FieldNoticesTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async void Get_Succeeds()
	{
		var _ =
			await CiscoClient.Pss.GetFieldNoticesAsync(
				new FieldNoticesRequest
				{
					CustomerId = Config.TestCustomerId,
					InventoryId = Config.TestInventoryId
				},
				CancellationToken.None
			)
			.ConfigureAwait(true);
	}
}