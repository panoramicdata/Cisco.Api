using Cisco.Api.Data.Pss;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

public class FieldNoticesTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async Task Get_Succeeds()
	{
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