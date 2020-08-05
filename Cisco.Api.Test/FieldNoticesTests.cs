using Cisco.Api.Data.Pss;
using System.Threading;
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
		public async void Get_Succeeds()
		{
			var ciscoFieldNoticesResponse =
				await CiscoClient.Pss.GetFieldNoticesAsync(
					new FieldNoticesRequest
					{
						CustomerId = "xxx",
						InventoryId = "xxx"
					},
					CancellationToken.None
				)
				.ConfigureAwait(false);
		}
	}
}
