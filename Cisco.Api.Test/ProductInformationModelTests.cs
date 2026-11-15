using Cisco.Api.Data.ProductInfo;
using Xunit;

namespace Cisco.Api.Test;

/// <summary>
/// Round-trip test for the ProductInformation data model, otherwise only exercised by integration tests that require live credentials.
/// </summary>
public class ProductInformationModelTests
{
	/// <summary>Every ProductInformation property is set and read back.</summary>
	[Fact]
	public void ProductInformation_EveryPropertyRoundTrips()
		=> PropertyAssert.SetAndRead<ProductInformation>(
			(nameof(ProductInformation.Dimensions), new Dimensions { Format = "10x10x10", Value = "cm" }),
			(nameof(ProductInformation.FormFactor), "ff"),
			(nameof(ProductInformation.Id), "i"),
			(nameof(ProductInformation.BasePid), "b"),
			(nameof(ProductInformation.OrderablePid), "o"),
			(nameof(ProductInformation.OrderableStatus), "O"),
			(nameof(ProductInformation.ProductCategory), "cat"),
			(nameof(ProductInformation.ProductName), "n"),
			(nameof(ProductInformation.ProductSeries), "s"),
			(nameof(ProductInformation.ProductSubcategory), "sc"),
			(nameof(ProductInformation.ProductSupportPage), "http://x"),
			(nameof(ProductInformation.ProductType), "t"),
			(nameof(ProductInformation.ReleaseDate), "2024-01-01"),
			(nameof(ProductInformation.RichMediaUrls), new RichMediaUrls { SmallImageUrl = "s1", LargeImageUrl = "l1" }),
			(nameof(ProductInformation.SerialNumber), "sn"),
			(nameof(ProductInformation.VisioStencilUrl), "http://v"),
			(nameof(ProductInformation.Weight), "1kg"),
			(nameof(ProductInformation.ErrorResponse), new ErrorResponse
			{
				ApiError = new ApiError { ErrorCode = "E", ErrorDescription = "d", SuggestedAction = "s" }
			}));
}
