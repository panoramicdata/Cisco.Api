using Cisco.Api.Data.ProductInfo;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Cisco.Api.Test;

public class ProductInformationTests(ITestOutputHelper iTestOutputHelper) : Test(iTestOutputHelper)
{
	[Fact]
	public async Task GetBySerialNumberAsync_Fails()
	{
		// Note: If no serials are found, then Products will have 1 empty record (yet totals say 0) that also has ErrorResponse set
		var productInformationPage = await CiscoClient
			.ProductInfo
			.GetBySerialNumbersAsync(["`"])
			.ConfigureAwait(true);

		productInformationPage.Should().NotBeNull();

		productInformationPage.PaginationResponseRecord.Should().NotBeNull();
		productInformationPage.PaginationResponseRecord.LastIndex.Should().Be(0);
		productInformationPage.PaginationResponseRecord.PageIndex.Should().Be(0);
		productInformationPage.PaginationResponseRecord.PageRecords.Should().Be(0);
		productInformationPage.PaginationResponseRecord.TotalRecords.Should().Be(0);

		productInformationPage.Products.Should().NotBeNull();
		productInformationPage.Products.Select(productInformation => productInformation.ErrorResponse).Should().NotBeNull();
	}

	[Fact]
	public async Task GetBySerialNumberAsync_Succeeds()
	{
		// Note: Serial numbers can be up to 40 chars long.
		var productInformationPage = await CiscoClient
			.ProductInfo
			.GetBySerialNumbersAsync(["FTX1910100B"])
			.ConfigureAwait(true);

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

	[Fact]
	public async Task GetBySerialNumberMultipleAsync_Succeeds()
	{
		// Note: Serial numbers can be up to 40 chars long.
		var productInformationPage = await CiscoClient
			.ProductInfo
			.GetBySerialNumbersAsync(["FCW2234L10F", "FCW2234L12V"])
			.ConfigureAwait(true);

		productInformationPage.Should().NotBeNull();

		productInformationPage.PaginationResponseRecord.Should().NotBeNull();
		productInformationPage.PaginationResponseRecord.LastIndex.Should().Be(0);
		productInformationPage.PaginationResponseRecord.PageIndex.Should().Be(0);
		productInformationPage.PaginationResponseRecord.PageRecords.Should().Be(0);
		productInformationPage.PaginationResponseRecord.TotalRecords.Should().Be(0);

		productInformationPage.Products.Count.Should().Be(2);
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

	[Fact]
	public async Task GetBySerialNumberMultipleAsyncTooLong_Succeeds()
	{
		// serial numbers is limited to 40 characters!
		var serialNumbers = new[] {
					"FCW2234L10F",
					"FCW2234L12V",
					"FCW2408P0LF",
					"FCW2408P0LP",
					"FCW2408P0LR",
					"FCW2408P0LS",
					"FCW2408P0LT",
					"FCW2408P0LW",
					"FCW2408P0M0",
					"FCW2408P0M2",
					"FCW2408P0M6",
					"FCW2408P0M9",
					"FCW2408P0MA",
					"FCW2408P0MC",
					"FCW2408P0MD",
					"FCW2408P0MF",
					"FCW2408P0MG",
					"FCW2408P0MJ",
					"FCW2408P0MK",
					"FCW2408P0ML",
					"FCW2234L10E",
					"FCW2234G122",
					"FCW2408P0MN",
					"FCW2408P0MP",
					"FCW2408P0MQ",
					"FCW2408P0MR",
					"FCW2408P0MS",
					"FCW2408P0MT",
					"FCW2408P0MU",
					"FCW2408P0MW",
					"FCW2408P0MX",
					"FCW2408P0MY",
					"FCW2408P0MZ",
					"FCW2408P0N0",
					"FCW2408P0N2",
					"FCW2408P0N3",
					"FCW2408P0N4",
					"FCW2408P0N5",
					"FCW2408P0N7",
					"FCW2408P0N8",
					"FOC2308U19E",
					"FCW2311L110",
					"FCW2408P0N9",
					"FCW2408P0NA",
					"FCW2408P0NB",
					"FCW2408P0NC",
					"FCW2408P0ND",
					"FCW2408P0NE",
					"FCW2408P0NF",
					"FCW2408P0NG",
					"FCW2408P0NH",
					"FCW2408P0NJ",
					"FCW2408P0NK",
					"FCW2408P0NL",
					"FCW2408P0NN",
					"FCW2408P0NP",
					"FCW2408P0NQ",
					"FCW2408P0NR",
					"FCW2408P0NS",
					"FCW2408P0NT"
				};

		ProductInformationPage productInformationPage;
		var basePids = new List<string>();
		foreach (var serialNumber in serialNumbers)
		{
			productInformationPage = await CiscoClient
			.ProductInfo
			.GetBySerialNumbersAsync([serialNumber])
			.ConfigureAwait(true);

			basePids.Add(productInformationPage.Products.First().BasePid);
		}

		basePids = [.. basePids.Distinct()];
		basePids.Count.Should().BeGreaterThan(1);
	}
}