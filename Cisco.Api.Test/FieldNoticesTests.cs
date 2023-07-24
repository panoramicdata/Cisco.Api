using System.Threading;
using System.Threading.Tasks;
using Cisco.Api.Data.Pss;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class FieldNoticesTests : Test
	{
		public FieldNoticesTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async Task Get_Succeeds()
		{
			_ =
				await CiscoClient.Pss.GetFieldNoticesAsync(
					new FieldNoticesRequest
					{
						CustomerId = Config.TestCustomerId,
						InventoryId = Config.TestInventoryId
					},
					CancellationToken.None
				)
				.ConfigureAwait(false);
		}
	}
}