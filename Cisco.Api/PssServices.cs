using Cisco.Api.Data.Pss;
using Cisco.Api.Interfaces;
using SimpleSOAPClient;
using SimpleSOAPClient.Helpers;
using SimpleSOAPClient.Models;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api
{
	public class PssServices : IPss
	{
		private readonly SoapClient _soapClient;

		public PssServices(HttpClient soapHttpClient)
		{
			_soapClient = SoapClient.Prepare(soapHttpClient);
		}

		public async Task<CustomersInventoryResponse> GetCustomerInventoryAsync(
			CustomersInventoryRequest request,
			CancellationToken cancellationToken)
		{
			var requestEnvelope = SoapEnvelope
				.Prepare()
				.Body(request);

			var responseEnvelope = await _soapClient.SendAsync(
				"InventoryService",
				"getCustomerInventoryIds",
				requestEnvelope,
				cancellationToken)
				.ConfigureAwait(false);

			return responseEnvelope
				.Body<CustomersInventoryResponse>();
		}
	}
}