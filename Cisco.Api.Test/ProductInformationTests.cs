using FluentAssertions;
using System.Linq;
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
		public async void GetBySerialNumberAsync_Succeeds()
		{
			var productInformationPage = await CiscoClient
				.ProductInfo
				.GetBySerialNumbersAsync(new[] { "FTX1910100B" })
				.ConfigureAwait(false);

			productInformationPage.Should().NotBeNull();

			productInformationPage.PaginationResponseRecord.Should().NotBeNull();
			productInformationPage.PaginationResponseRecord.LastIndex.Should().Be(0);
			productInformationPage.PaginationResponseRecord.PageIndex.Should().Be(0);
			productInformationPage.PaginationResponseRecord.PageRecords.Should().Be(0);
			productInformationPage.PaginationResponseRecord.TotalRecords.Should().Be(0);

			productInformationPage.Products.Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.Dimensions).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.Dimensions.Format).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.Dimensions.Value).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.FormFactor).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.Id).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.BasePid).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.OrderablePid).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.OrderableStatus).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ProductCategory).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ProductName).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ProductSeries).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ProductSubcategory).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ProductSupportPage).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.ReleaseDate).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.RichMediaUrls).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.RichMediaUrls.LargeImageUrl).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.RichMediaUrls.SmallImageUrl).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.SerialNumber).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.VisioStencilUrl).Should().NotBeNull();
			productInformationPage.Products.Select(productInformation => productInformation.Weight).Should().NotBeNull();
		}
	}
}