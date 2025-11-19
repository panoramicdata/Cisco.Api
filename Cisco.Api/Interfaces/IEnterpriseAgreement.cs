using Cisco.Api.Data.EnterpriseAgreement.Responses;
using Refit;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api.Interfaces;

public interface IEnterpriseAgreement
{
	/// https://apidocs-prod.cisco.com/explore;category=611719fa4e429c207fef9a59 (requires login with Cisco ID granted access)

	////////////////////////////////////////
	//// EA Consumption Report APIs

	/// <summary>
	/// This API can be used to get the consumption report for All EA Subscriptions asscociated with given smart account domain.
	/// </summary>
	/// <param name="smartAccountDomain">Domain of the smart account.</param>
	/// <param name="cancellationToken"></param>
	[Get("/v1/subscription/account/{smartAccountDomain}/consumption")]
	Task<EaSmartAccountAllSubscriptionConsumptionReportResponse> GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync(
		string smartAccountDomain,
		CancellationToken cancellationToken = default);
}
