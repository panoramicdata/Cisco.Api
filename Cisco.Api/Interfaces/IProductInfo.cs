using Cisco.Api.Data.ProductInfo;
using Refit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// Product Info calls
	/// </summary>
	public interface IProductInfo
	{
		/// <summary>
		/// Gets product information by serial numbers
		/// </summary>
		/// <param name="serialNumbers">The serial numbers</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The coverage status</returns>
		[Get("/product/v1/information/serial_numbers/{serialNumbers}")]
		Task<ProductInformationPage> GetBySerialNumbersAsync(
			[Query(CollectionFormat.Csv)] IEnumerable<string> serialNumbers,
			CancellationToken cancellationToken = default);
	}
}