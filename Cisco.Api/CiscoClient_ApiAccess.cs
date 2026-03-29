using Cisco.Api.Data.Pss;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cisco.Api;
public partial class CiscoClient
{
	/// <summary>
	/// This method tries each Interface to see if the provided credentials are valid
	/// </summary>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task<ApiAccess> GetApiAccessAsync(CancellationToken cancellationToken)
	{
		_logger.LogDebug("Checking API access...");
		var apiAccess = new ApiAccess();
		var todayMidnight = DateTime.Today;
		var yesterdayMidnight = todayMidnight.AddDays(-1);

		// MS-19906 You can search specifically for the demo domain and in that specific case you may get an error depending on your perms but it's a 200
		var enterpriseAgreementTask = EnterpriseAgreement.GetConsumptionReportForAllSubscriptionsAssociatedWithSmartAccountDomainAsync("demo.mule.cisco.com", cancellationToken);
		var eoxTask = Eox.GetByDatesAsync(yesterdayMidnight, todayMidnight, 1, cancellationToken);
		var helloTask = Hello.HelloAsync(cancellationToken);
		var psirtTask = Psirt.GetLatestAsync(1, cancellationToken);
		// MS-19906 You cannot use an empty list - you get an API error, but you CAN make one up and if this is supported we get a response
		var productInfoTask = ProductInfo.GetBySerialNumbersAsync(["123"], cancellationToken);
		var pssTask = Pss.GetFieldNoticesAsync(new FieldNoticesRequest(), cancellationToken);
		// MS-19906 You cannot use an empty list - you get an API error, but you CAN make one up and if this is supported we get a response
		var serialNumberToInfoTask = SerialNumberToInfo.GetCoverageStatusBySerialNumbersAsync(["123"], cancellationToken);
		var softwareSuggestionTask = SoftwareSuggestion.GetByProductIdsAsync(["C9200"], 1, cancellationToken);
		// MS-19906 You can search for anything
		var smartAccountsAndLicensingTask = SmartAccountsAndLicensing.SearchSmartAccountsAsync(name: "123", cancellationToken: cancellationToken);
		var umbrellaTask = Umbrella.ListSitesAsync(cancellationToken: cancellationToken);

		apiAccess.EnterpriseAgreement = await TryApiAsync("Enterprise Agreement", enterpriseAgreementTask).ConfigureAwait(false);
		apiAccess.Eox = await TryApiAsync("Eox", eoxTask).ConfigureAwait(false);
		apiAccess.Hello = await TryApiAsync("Hello", helloTask).ConfigureAwait(false);
		apiAccess.Psirt = await TryApiAsync("Psirt", psirtTask).ConfigureAwait(false);
		apiAccess.ProductInfo = await TryApiAsync("ProductInfo", productInfoTask).ConfigureAwait(false);
		apiAccess.Pss = await TryApiAsync("Pss", pssTask).ConfigureAwait(false);
		apiAccess.SerialNumberToInfo = await TryApiAsync("SerialNumberToInfo", serialNumberToInfoTask).ConfigureAwait(false);
		apiAccess.SoftwareSuggestion = await TryApiAsync("SoftwareSuggestion", softwareSuggestionTask).ConfigureAwait(false);
		apiAccess.SmartAccountsAndLicensing = await TryApiAsync("SmartAccountsAndLicensing", smartAccountsAndLicensingTask).ConfigureAwait(false);
		apiAccess.Umbrella = await TryApiAsync("Umbrella", umbrellaTask).ConfigureAwait(false);

		return apiAccess;
	}

	private async Task<bool> TryApiAsync(string name, Task task)
	{
		try
		{
			_logger.LogDebug("Checking {Name}", name);
			await task.ConfigureAwait(false);
			_logger.LogDebug("{Name} succeeded", name);
			return true;
		}
		catch (Exception ex)
		{
			_logger.LogDebug(ex, "{Name} failed", name);
			return false;
		}
	}
}
