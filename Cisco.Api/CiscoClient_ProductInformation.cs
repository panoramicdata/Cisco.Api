using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public partial class CiscoClient
	{
		/// <summary>
		/// Gets product information for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The product information</returns>
		public async Task<ProductInformation> GetProductInformationBySerialNumber(string serialNumber, CancellationToken cancellationToken = default)
			=> (await GetAsync<ProductInformationPage>($"product/v1/information/serial_numbers/{serialNumber}", cancellationToken).ConfigureAwait(false)).Products.FirstOrDefault();

		/// <summary>
		/// Gets product information for multiple serial numbers
		/// </summary>
		/// <param name="serialNumbers">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>A list of product information objects</returns>
		public async Task<List<ProductInformation>> GetProductInformationBySerialNumbers(List<string> serialNumbers, CancellationToken cancellationToken = default)
			=> (await GetAsync<ProductInformationPage>($"product/v1/information/serial_numbers/{string.Join(",", serialNumbers)}", cancellationToken).ConfigureAwait(false)).Products;
	}
}
