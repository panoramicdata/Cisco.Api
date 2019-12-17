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
			var productInformation = await CiscoClient.GetProductInformationBySerialNumber("FTX1910100B");
			Assert.NotNull(productInformation);
			Assert.NotNull(productInformation.BasePid);
			Assert.NotNull(productInformation.Dimensions);
			Assert.NotNull(productInformation.Dimensions.Format);
			Assert.NotNull(productInformation.Dimensions.Value);
			Assert.NotNull(productInformation.FormFactor);
			Assert.NotNull(productInformation.Id);
			Assert.NotNull(productInformation.OrderablePid);
			Assert.NotNull(productInformation.OrderableStatus);
			Assert.NotNull(productInformation.ProductCategory);
			Assert.NotNull(productInformation.ProductName);
			Assert.NotNull(productInformation.ProductSeries);
			Assert.NotNull(productInformation.ProductSubcategory);
			Assert.NotNull(productInformation.ProductSupportPage);
			Assert.NotNull(productInformation.ReleaseDate);
			Assert.NotNull(productInformation.RichMediaUrls);
			Assert.NotNull(productInformation.RichMediaUrls.LargeImageUrl);
			Assert.NotNull(productInformation.RichMediaUrls.SmallImageUrl);
			Assert.NotNull(productInformation.SerialNumber);
			Assert.NotNull(productInformation.VisioStencilUrl);
			Assert.NotNull(productInformation.Weight);
		}
	}
}