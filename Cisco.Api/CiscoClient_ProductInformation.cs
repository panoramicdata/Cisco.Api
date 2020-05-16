using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public partial class CiscoClient
	{
		/// <summary>
		/// Gets coverage status for a single serial number
		/// </summary>
		/// <param name="serialNumber">The serial number</param>
		/// <param name="cancellationToken">An optional cancellation token</param>
		/// <returns>The coverage status</returns>
		public async Task<ProductInformation> GetProductInformationBySerialNumber(string serialNumber, CancellationToken cancellationToken = default)
			=> (await GetAsync<ProductInformationPage>($"product/v1/information/serial_numbers/{serialNumber}", cancellationToken).ConfigureAwait(false)).Products.FirstOrDefault();
	}
}
