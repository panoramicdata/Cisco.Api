using Cisco.Api.Data.Pss;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Contains tests for field notices operations.
/// </summary>
/// <param name="iTestOutputHelper">The test output helper.</param>
public class FieldNoticesTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	/// <summary>
	/// Verifies the get succeeds scenario.
	/// </summary>
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