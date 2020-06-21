using Cisco.Api.Data.Pss;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces
{
	/// <summary>
	/// PSS calls
	/// </summary>
	public interface IPss
	{
		Task<CustomersInventoryResponse> GetCustomerInventoryAsync(
			CustomersInventoryRequest customerInventoryRequest,
			CancellationToken cancellationToken);
	}
}