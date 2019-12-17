using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test
{
	public class ProductInformationTests : Test
	{
		public ProductInformationTests(ITestOutputHelper iTestOutputHelper) : base(iTestOutputHelper)
		{
		}

		[Fact]
		public async void GetProductInformationBySerialNumber()
		{
			var productInformation = await CiscoClient.GetProductInformationBySerialNumber("FTX1910100B").ConfigureAwait(false);
			Assert.NotNull(productInformation);
		}
	}
}