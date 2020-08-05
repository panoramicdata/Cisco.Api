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
						CustomerId = "140194686",
						InventoryId = "580328"
					},
					CancellationToken.None
				)
				.ConfigureAwait(false);
		}
	}
}
